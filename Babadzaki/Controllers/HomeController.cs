using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.Utility;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

//using System.Text.Json;

namespace Babadzaki.Controllers
{
    [Consumes("application/x-www-form-urlencoded")]
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

        ////[HttpPost]
        ////[ValidateAntiForgeryToken]//TODO: разобраться как работает
        ////public IActionResult IndexPost(HomeVM homeVM)//отправка email
        ////{
        ////    if (!ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
        ////    {
        ////        var email = _context.Emails.Where(e => e.Name == homeVM.Email.Name).FirstOrDefault();
        ////        if (homeVM.Email != null && email != null)
        ////        {
        ////            _context.Emails.Add(homeVM.Email);
        ////            _mailService.SendMessage(homeVM.Email.Name, "test", "test");
        ////        }
        ////    }\

        ////    return RedirectToAction(nameof(Index));
        ////}
        ////[Consumes("application/json")]
        //[HttpPost(Name = "JsonPostEmailSend")]
        ////[Route("Home/JsonPostEmailSend")]
        //public void JsonPostEmailSend([FromBody] Email email)
        //{

        //    _logger.LogError("Hyu");

        //}
        [HttpPost]

        //[Route("Home/JsonPostEmailSend")]
        public async Task<JsonResult> JsonPostEmailSendAsync([FromForm]Email  email)
        {

            _logger.LogWarning("Hyu");

            if (ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
            {
                var _email = await _context.Emails.Where(e => e.Name == email.Name).FirstOrDefaultAsync();
                if (email != null && _email == null)
                {
                    await _context.Emails.AddAsync(email);//TODO: поменять местами
                    await _context.SaveChangesAsync();

                }
                _mailService.SendMessage(to:email.Name, subject:"test",bodyText: "test");

            }
            return new JsonResult(Ok(email));
        }

        
    }

}
