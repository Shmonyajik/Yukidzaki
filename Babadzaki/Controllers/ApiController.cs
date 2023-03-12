using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.Utility;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Babadzaki.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;
        public ApiController(ILogger<ApiController> logger, ApplicationDbContext context, IMailService mailService) 
        {
            _logger = logger;
            _context= context;
            _mailService= mailService;
        }
        [HttpPost]
        
        //[Route("Home/JsonPostEmailSend")]
        public async Task<JsonResult> JsonPostEmailSendAsync([FromForm] Email email)
        {

            _logger.LogWarning("Hyu");

            if (ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
            {
                var _email = await _context.Emails.Where(e => e.Name == email.Name).FirstOrDefaultAsync();
                if (email != null && _email == null)
                {
                    await _context.Emails.AddAsync(email);
                    await _context.SaveChangesAsync();
                    
                }
                _mailService.SendMessage(email.Name, "test", "test");

            }
            return new JsonResult(Ok(email));
        }
        [HttpPost]
        public async Task<JsonResult> JsonPostQuestionSendAsync([FromForm] QuestionVM questionVM)
        {

            _logger.LogWarning("Hyu");

            if (!ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
            {

                if (questionVM.Email != null)
                {
                    var _email = await _context.Emails.Where(e => e.Name == questionVM.Email).FirstOrDefaultAsync();
                    if (_email != null)
                    {
                        await _context.Emails.AddAsync(new Email { Name = questionVM.Email });
                        await _context.SaveChangesAsync();
                    }
                    _mailService.SendMessage(WebConstants.EmailFrom, "test", "test");
                    _mailService.SendMessage(questionVM.Email, "test", "test");
                }

            }
            return new JsonResult(Ok(questionVM));
        }
    }
}
