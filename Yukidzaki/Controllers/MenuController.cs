using Microsoft.AspNetCore.Mvc;

namespace Yukidzaki.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult NotFoundError()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ComingSoon()
        {
            return View();
        }
        public IActionResult Lore()
        {
            return View();
        }
        public IActionResult Manga()
        {
            return View();
        }
        public IActionResult Roadmap()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }


    }
}
