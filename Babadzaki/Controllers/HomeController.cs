using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.Utility;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            _context= context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Tokens = _context.Tokens.Include(u => u.SeasonCollection),
                SeasonCollections = _context.SeasonCollections
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

        public IActionResult SendEmail()
        {
            ViewBag.MailService = _mailService;
            return View();
        }
        public IActionResult SendDefault()
        {
            _mailService.SendMessage();
            return RedirectToAction(nameof(SendEmail));
        }
        public IActionResult SendCustom()
        {
            _mailService.SendMessage();
            return RedirectToAction(nameof(SendEmail));
        }
    }
}