using Babadzaki.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Babadzaki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        public ApiController(ILogger<ApiController> logger) 
        {
            _logger = logger;
        }
        [HttpPost]
        //[Route("Home/JsonPostEmailSend")]
       
        public JsonResult JsonPostEmailSend([FromForm] Email email)
        {

            _logger.LogWarning("Hyu");

            return new JsonResult(Ok());

        }
    }
}
