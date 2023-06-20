using Babadzaki_Serivces.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Babadzaki.Controllers
{
    public class Test : Controller
    {
        private readonly IGalleryService _galleryService;
        public Test(IGalleryService galleryService) 
        {
            _galleryService = galleryService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var response = await _galleryService.GetGallery();
            if(response.StatusCode == Babadzaki_Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
    }
}
