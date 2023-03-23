using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki_Utility;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace Babadzaki.Controllers
{
    [Consumes("application/x-www-form-urlencoded")]
    public class FeedbackController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public FeedbackController(ILogger<HomeController> logger, ApplicationDbContext context, IMailService mailService)
        {
            _mailService = mailService;
            _context = context;
            _logger = logger;
        }
        public ActionResult Index()
        {
            
            return View(new FeedbackVM());
        }
        [HttpPost]
        public async Task<JsonResult> JsonPostQuestionSendAsync([FromForm] FeedbackVM questionVM)
        {

            _logger.LogWarning("Hyu");

            if (ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
            {

                if (questionVM.Email != null)
                {
                    var _email = await _context.Emails.Where(e => e.Name == questionVM.Email).FirstOrDefaultAsync();
                    if (_email != null)
                    {
                        await _context.Emails.AddAsync(new Email { Name = questionVM.Email });
                        await _context.SaveChangesAsync();
                    }
                    _mailService.SendMessage(WebConstants.EmailFrom,questionVM.Email, "Question", questionVM.Message);
                    _mailService.SendMessage(questionVM.Email, "Question", "Thanks for your question!");
                }

            }
            return new JsonResult(Ok(questionVM));
        }
    }
}
