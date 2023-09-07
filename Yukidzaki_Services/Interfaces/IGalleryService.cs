
using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Domain.ViewModels;

namespace Yukidzaki_Services.Interfaces
{
    public interface IGalleryService
    {
        Task<BaseResponse<GalleryVM>> GetGallery();

        Task<BaseResponse<Token>> GetToken(int id);

        Task<BaseResponse<FilterVM>> GetTokensByFilters(IEnumerable<TokensFilters> tokensFilters, int id);
    }
}
