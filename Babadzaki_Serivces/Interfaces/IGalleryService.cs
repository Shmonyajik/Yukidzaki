
using Babadzaki_Domain.Models;
using Babadzaki_Domain.Responses;
using Babadzaki_Domain.ViewModels;

namespace Babadzaki_Serivces.Interfaces
{
    public interface IGalleryService
    {
        Task<BaseResponse<GalleryVM>> GetGallery();

        Task<BaseResponse<Token>> GetToken(int id);

        Task<BaseResponse<FilterVM>> GetTokensByFilters(IEnumerable<TokensFilters> tokensFilters, int id);
    }
}
