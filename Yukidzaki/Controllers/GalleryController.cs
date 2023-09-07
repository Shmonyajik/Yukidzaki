using Yukidzaki.ViewModel;
using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Domain.ViewModels;
using Yukidzaki_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Yukidzaki.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryService _galleryService;
        private List<Token> tokens;
        public GalleryController(IGalleryService galleryService) 
        {
            _galleryService = galleryService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var response = await _galleryService.GetGallery();
            if(response.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> TokenDetails(int id)
        {
            var response = await _galleryService.GetToken(id);
            if(response.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK)
            {
                return PartialView("_ModalTokenDetails", response.Data);
            }

            return View("Error", $"{response.Description}");
        }

        [HttpPost]
        public async Task<ActionResult> Filter([FromBody] FilterPageVM filterPageVM)//в гет нельзя передавать тело, сделаем оба пост
        {
            
            var response = await _galleryService.GetTokensByFilters(filterPageVM.tokensFilters, filterPageVM.page);


            if (response.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK)
            {
                if(response.Description=="Loading")
                {
                    return PartialView("_NextPage", response.Data.Tokens);
                }
                return PartialView("_TokenCardGallery", response.Data);
            }

            return View("Error", $"{response.Description}");
            

        }
    }
}
