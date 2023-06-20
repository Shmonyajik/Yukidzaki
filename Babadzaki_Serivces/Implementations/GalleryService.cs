using Babadzaki_DAL.Interfaces;
using Babadzaki_DAL.Repositories;
using Babadzaki_Domain.Enums;
using Babadzaki_Domain.Models;
using Babadzaki_Domain.Responses;
using Babadzaki_Domain.ViewModels;
using Babadzaki_Serivces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_Serivces.Implementations
{
    public class GalleryService : IGalleryService
    {
        private readonly IBaseRepository<Token> _tokenRepository;
        private readonly IBaseRepository<SeasonCollection> _seasonCollectionRepository;
        private readonly IBaseRepository<TokensFilters> _tokensFiltersRepository;
        private readonly IBaseRepository<Filter> _filterRepository;
        public GalleryService(IBaseRepository<Token> tokenRepository,
                IBaseRepository<SeasonCollection> seasonCollectionRepository,
                IBaseRepository<TokensFilters> tokensFiltersRepository,
                IBaseRepository<Filter> filterRepository
            )
        {
            _tokenRepository = tokenRepository;
            _seasonCollectionRepository = seasonCollectionRepository;
            _tokensFiltersRepository = tokensFiltersRepository;
            _filterRepository = filterRepository;
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

        public Task<BaseResponse<Token>> GetToken(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<FilterVM>> GetTokensByFilters(IEnumerable<TokensFilters> tokensFilters)
        {
            throw new NotImplementedException();
        }
    }
}
