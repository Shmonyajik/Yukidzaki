using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Services.Implementations;
using Yukidzaki_Services.Interfaces;
using Babadzaki_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Yukidzaki.Controllers
{
    [Consumes("application/x-www-form-urlencoded")]
    public class HomeController : Controller
    {
        public readonly IHomeService _homeService;
        public readonly IMailService _mailService;
        public readonly IConfiguration _configuration;

        
        public HomeController(IHomeService homeService, IMailService mailService, IConfiguration configuration)
        {
            _mailService = mailService;
            _homeService = homeService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var response = await _homeService.GetTokens();

            if (response.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
        [HttpPost]

        public async Task<JsonResult> JsonPostEmailSendAsync([FromForm] string emailName)
        {
            var email = new Email { Name = emailName };
            if (ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
            {
                
                var sendMessageResponse = await _mailService.SendMessage(
                    email,
                    _configuration.GetSection("EmailSettings:EmailName").Value,
                    WebConstants.SubscribeSubject,
                    WebConstants.SubscribeMessage
                );

                if (sendMessageResponse.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK)
                {
                    var SaveEmailResponse = await _homeService.SaveEmail(email);
                    return Json(SaveEmailResponse);
                }
                else
                    return Json(sendMessageResponse);
                
            }
            var invalidFields = ModelState.Values
               .Where(v => v.ValidationState == ModelValidationState.Invalid)
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();
            return Json(new BaseResponse<List<string>> {
                Data = invalidFields,
                Description = $"Model state is invalid",
                StatusCode = Yukidzaki_Domain.Enums.StatusCode.ModelStateIsInvalid
            });
        }


    }
}
