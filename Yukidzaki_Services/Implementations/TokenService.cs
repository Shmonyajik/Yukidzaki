using AutoMapper;

using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Services.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Yukidzaki_DAL.Interfaces;
using Yukidzaki_Domain.Enums;

using Microsoft.AspNetCore.Http;
using Yukidzaki_Domain.Mappings;

namespace Yukidzaki_Services.Implementations
{
    public class TokenService : ITokenService
    {
        
        //private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Token> _tokenRepository;
        private readonly IBaseRepository<SeasonCollection> _seasonCollectionRepository;
        private readonly IBaseRepository<Filter> _filterRepository;
        public TokenService(IBaseRepository<Token> tokenRepository, IMapper mapper, IBaseRepository<SeasonCollection> seasonCollectionRepository, IBaseRepository<Filter> filterRepository)//, IBaseRepository<SeasonCollection> seasonCollectionRepository
        {
            _tokenRepository = tokenRepository;
            _mapper = mapper;
            _seasonCollectionRepository = seasonCollectionRepository;
            _filterRepository = filterRepository;
                
        }
        public async Task<BaseResponse<IEnumerable<Token>>> GetToken()
        {
            var baseResponse = new BaseResponse<IEnumerable<Token>>();
            try
            {
                var tokens =  await _tokenRepository.GetAll().ToListAsync();//Include(t=>t.SeasonCollection).

                baseResponse.Data = (IEnumerable<Token>)tokens;

                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Token>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> LoadTokens(IFormFileCollection files)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        baseResponse.Description = "One or more files are empty";
                        baseResponse.StatusCode = StatusCode.JsonReaderError;
                        return baseResponse;
                    }
                    
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var fileString = await reader.ReadToEndAsync();
                        JsonToken jsonToken = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonToken>(fileString);
                        SeasonCollection seasonCollection = null;
                        Token token = _mapper.Map<Token>(jsonToken);
                        if (await _tokenRepository.GetAll().FirstOrDefaultAsync(t => t.edition == token.edition) != null)
                        {
                            reader.Close();
                            continue;
                        }

                        foreach (var attr in jsonToken.attributes)
                        {
                            if (attr.trait_type == "Season")
                            {
                                try
                                {
                                    token.SeasonCollection = await _seasonCollectionRepository.GetAll().FirstAsync(sc => sc.Name == attr.value);
                                }
                                catch (Exception)
                                {
                                    token.SeasonCollection = new SeasonCollection { Name = attr.value };
                                }
                                
                                continue;
                            }
                            //List<Filter> filter = null;
                            //filter = await _filterRepository.GetAll().ToListAsync(); /*(f => f.Name == attr.trait_type);*/
                            //try
                            //{
                            //    filter = await _filterRepository.GetAll().ToListAsync(); /*(f => f.Name == attr.trait_type);*/
                            //}
                            //catch (NullReferenceException)
                            //{
                            //    filter = new Filter
                            //    {
                            //        Name = attr.trait_type
                            //    };
                            //}
                            //token.TokensFilters.Add(new TokensFilters
                            //{
                            //    Value = attr.value,
                            //    Filter = filter
                            //});
                            //try
                            //{
                            //    token.TokensFilters.Add(new TokensFilters
                            //    {
                            //        Value = attr.value,
                            //        Filter = await _filterRepository.GetAll().FirstOrDefaultAsync(f => f.Name == attr.trait_type) ?? new Filter { Name = attr.trait_type }
                            //    });
                            //}
                            //catch (NullReferenceException)
                            //{

                            //    throw;
                            //}
                            token.TokensFilters.Add(new TokensFilters
                            {
                                Value = attr.value,
                                Filter = await _filterRepository.GetAll().FirstOrDefaultAsync(f => f.Name == attr.trait_type) ?? new Filter { Name = attr.trait_type }
                            });


                        }

                        await _tokenRepository.Create(token);
                    }
                }
                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = true;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
