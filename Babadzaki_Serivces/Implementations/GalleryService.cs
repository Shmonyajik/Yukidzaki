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
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

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

                    TokensFilters = await _tokensFiltersRepository.GetAll().///неработает
                    Include(f => f.Filter).GroupBy(tf => new { tf.Value, tf.Filter.Name}).
                    Select(x =>new TokensFilters{
                        Value = x.Key.Value,
                        Filter = new Filter {Name = x.Key.Name } 
                    }).
                    ToListAsync(),

                    Filters = await _filterRepository.GetAll().ToListAsync()
                };

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
                    filterVM.tokensCount = tokens.Count();
                    filterVM.Tokens = GetTokensPage(tokens, id);
                    baseResponse.Data = filterVM;
                    baseResponse.Description = "Loading";
                    baseResponse.StatusCode = StatusCode.OK;
                    return baseResponse;
                }
                if (tokensFilters is null || tokensFilters.Count()==0)
                {
                    tokensFinded = await _tokenRepository.GetAll().ToListAsync();
                    _cache.Set(0, tokensFinded, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    filterVM.tokensCount = tokensFinded.Count();
                    filterVM.Tokens = tokensFinded.Take(WebConstants.PageOfTokensSize);    
                    baseResponse.StatusCode = StatusCode.OK;
                    baseResponse.Data = filterVM;
                       
                    return baseResponse;
                }
                
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
    }
}
