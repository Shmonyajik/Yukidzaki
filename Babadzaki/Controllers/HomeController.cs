using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.Utility;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace Babadzaki.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMailService mailService)
        {
            _mailService = mailService;
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Tokens = _context.Tokens.Include(u => u.SeasonCollection),
                SeasonCollections = _context.SeasonCollections,
                Email = new Email()

            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//TODO: разобраться как работает
                                  //public IActionResult IndexPost(HomeVM homeVM)//отправка email
                                  //{
                                  //    if (!ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
                                  //    {
                                  //        var email = _context.Emails.Where(e => e.Name == homeVM.Email.Name).FirstOrDefault();
                                  //        if (homeVM.Email != null && email != null)
                                  //        {
                                  //            _context.Emails.Add(homeVM.Email);
                                  //            _mailService.SendMessage(homeVM.Email.Name, "test", "test");
                                  //        }
                                  //    }

        //    return RedirectToAction(nameof(Index));
        //}
        //[Route("/HomeController/JsonWriteEmail")]
        [HttpPost]
        public JsonResult JsonWriteEmail(string email)
        {
            if (!ModelState.IsValid&& email != ""/*&&homeVM.Email.Name!=null*/)
            {
                var _email = _context.Emails.Where(e => e.Name == email).FirstOrDefault();
                if (email != "" && _email != null)
                {
                    _context.Emails.Add(new Email {Name= email });
                    _mailService.SendMessage(email, "test", "test");
                    return Json(Ok());
                }
            }

            return Json(Ok());
        }
    }
}