using Babadzaki_DAL.Interfaces;
using Babadzaki_DAL.Repositories;
using Babadzaki_Domain.Enums;
using Babadzaki_Domain.Models;
using Babadzaki_Domain.Responses;
using Babadzaki_Domain.ViewModels;
using Babadzaki_Serivces.Interfaces;
using Babadzaki_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Babadzaki_Serivces.Implementations
{
    public class GalleryService : IGalleryService
    {
        private readonly IBaseRepository<Token> _tokenRepository;
        private readonly IBaseRepository<SeasonCollection> _seasonCollectionRepository;
        private readonly IBaseRepository<TokensFilters> _tokensFiltersRepository;
        private readonly IBaseRepository<Filter> _filterRepository;
        //private FilterVM filterVM = new FilterVM();
        
        private IMemoryCache _cache;
        public GalleryService(IBaseRepository<Token> tokenRepository,
                IBaseRepository<SeasonCollection> seasonCollectionRepository,
                IBaseRepository<TokensFilters> tokensFiltersRepository,
                IBaseRepository<Filter> filterRepository,
                IMemoryCache cache
            )
        {
            _tokenRepository = tokenRepository;
            _seasonCollectionRepository = seasonCollectionRepository;
            _tokensFiltersRepository = tokensFiltersRepository;
            _filterRepository = filterRepository;
            _cache = cache;
        }
        public async Task<BaseResponse<GalleryVM>> GetGallery()
        {
            var baseResponse = new BaseResponse<GalleryVM>();
            try
            {
                GalleryVM galleryVM = new GalleryVM
                {
                    SeasonCollections = await _seasonCollectionRepository.GetAll().ToListAsync(),

                    TokensFilters = await _tokensFiltersRepository.GetAll()
                               .Include(f => f.Filter)
                               .Select(tf => new
                               {
                                   tf.Id, // Добавляем Id записей
                                   tf.Value,
                                   tf.Filter.Name
                               })
                               .GroupBy(x => new { x.Value, x.Name })
                               .Select(x => new TokensFilters
                               {
                                   Id = x.Select(y => y.Id).First(), // Выбираем Id из первой записи в группе
                                   Value = x.Key.Value,
                                   Filter = new Filter { Name = x.Key.Name }
                               })
                               .ToListAsync(),

                    Filters = await _filterRepository.GetAll().ToListAsync(),
                    TokensWithAttributeCount = new Dictionary<int, int>()
                    
                };
                
                var tf = await _tokensFiltersRepository.GetAll().ToListAsync();

                
                foreach (var item in galleryVM.TokensFilters)
                {
                    galleryVM.TokensWithAttributeCount
                        .Add(item.Id, tf.Where(tf => tf.Value == item.Value && tf.Filter.Name == item.Filter.Name).Count());
                }    

                baseResponse.Data = galleryVM;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;

            }
            catch (Exception ex)
            {
                return new BaseResponse<GalleryVM>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };

            }
        }

        public async Task<BaseResponse<Token>> GetToken(int id)
        {
            var baseResponse = new BaseResponse<Token>();
            try
            {
                 
                var token = await _tokenRepository.GetAll().
                    Include(u => u.SeasonCollection).
                    Include(f => f.TokensFilters).ThenInclude(tf => tf.Filter).
                    FirstOrDefaultAsync(x => x.edition == id);
                if (token != null)
                {
                    baseResponse.Data = token;
                    baseResponse.StatusCode = StatusCode.OK;
                }
                else
                {
                    baseResponse.StatusCode = StatusCode.NotFound;
                    baseResponse.Description = $"Token with edition {id} not found";
                }
                return baseResponse;
            }
            catch (Exception ex)
            {

                return new BaseResponse<Token>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
            
        }
       
        public async Task<BaseResponse<FilterVM>> GetTokensByFilters(IEnumerable<TokensFilters> tokensFilters, int id)
        {
            var baseResponse = new BaseResponse<FilterVM>();
            FilterVM filterVM = new FilterVM();

            try
            {
                IEnumerable<Token> tokensFinded = null;
                if (id!=0)
                {
                    _cache.TryGetValue(0, out IEnumerable<Token> tokens);
                    tokens = RandomizeTokes(tokens.ToList());
                    filterVM.tokensCount = tokens.Count();
                    filterVM.Tokens = GetTokensPage(tokens, id);
                    baseResponse.Data = filterVM;
                    baseResponse.Description = "Loading";
                    baseResponse.StatusCode = StatusCode.OK;
                    return baseResponse;
                }
                if (tokensFilters is null || tokensFilters.Count()==0)
                {
                    tokensFinded = RandomizeTokes( await _tokenRepository.GetAll().ToListAsync());
                    _cache.Set(0, tokensFinded, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(25)));
                    filterVM.tokensCount = tokensFinded.Count();
                    filterVM.Tokens = tokensFinded.Take(WebConstants.PageOfTokensSize).ToList();    
                    baseResponse.StatusCode = StatusCode.OK;
                    baseResponse.Data = filterVM;
                       
                    return baseResponse;
                }
                //_cache.Remove(0);
                string searchQuery = null;

                foreach (var filter in tokensFilters)
                {
                    if (filter.FilterId == 0)
                    {
                        searchQuery = filter.Value;
                        continue;
                    }
                    if (tokensFinded == null)
                    {   
                        
                        tokensFinded =  await _tokenRepository.GetAll().Where(t => t.TokensFilters.
                        FirstOrDefault(tf => tf.Value == filter.Value && tf.Filter.Id == filter.FilterId) != null).
                        Include(tf => tf.TokensFilters).ThenInclude(f => f.Filter).ToListAsync();
                    }
                    else
                        tokensFinded = tokensFinded.Where(
                            t => t.TokensFilters.FirstOrDefault(
                                tf => tf.Value == filter.Value && tf.Filter.Id == filter.FilterId) != null).
                                ToList();

                }
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    if (tokensFinded == null)
                        tokensFinded = await  _tokenRepository.GetAll().Where(t => t.edition.ToString() == searchQuery).ToListAsync();
                    
                    else
                    {
                        tokensFinded = tokensFinded.Where(t => t.edition.ToString() == searchQuery).ToList();
                    }
                }
                tokensFinded = RandomizeTokes(tokensFinded.ToList());
                _cache.Set(0, tokensFinded, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                filterVM.Tokens = tokensFinded.Take(WebConstants.PageOfTokensSize).ToList();
                
                filterVM.tokensCount = tokensFinded.Count();

                
                
                //return PartialView("_TokenCardGallery", tokens);
                baseResponse.Data = filterVM;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<FilterVM>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private List<Token> GetTokensPage(IEnumerable<Token> tokens, int page = 1)
        {
            var itemsToSkip = page * WebConstants.PageOfTokensSize;//Вынести в вебконстанты

            return tokens.Skip(itemsToSkip).
                Take(WebConstants.PageOfTokensSize).ToList();
        }

        private List<Token> RandomizeTokes(List<Token> tokens)
        {
            Random random = new Random();
            for (int i = tokens.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                // обменять значения data[j] и data[i]
                var temp = tokens[j];
                tokens[j] = tokens[i];
                tokens[i] = temp;
            }
            return tokens;
        }

    }
}
