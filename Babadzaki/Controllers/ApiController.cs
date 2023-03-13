using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Babadzaki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public ApiController(ILogger<ApiController> logger, ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _logger = logger;
            _mailService = mailService;
        }
        [HttpPost]
        //[Route("Home/JsonPostEmailSend")]
       
        public async Task JsonPostEmailSendAsync([FromForm] Email email)
        {

            _logger.LogWarning("Hyu");

            if (!ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
            {
                var _email = await _context.Emails.Where(e => e.Name == email.Name).FirstOrDefaultAsync();
                if (email.Name != null && email != null)
                {
                    await _context.Emails.AddAsync(email);
                    await _context.SaveChangesAsync();
                    _mailService.SendMessage(email.Name, "test", "test");
                }
            }

        }
    }
}
