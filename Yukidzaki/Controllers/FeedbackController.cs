

using Microsoft.AspNetCore.Mvc;
using Yukidzaki_Services.Interfaces;
using Yukidzaki_Domain.ViewModels;
using Babadzaki_Services;
using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Yukidzaki.Controllers
{
    [Consumes("application/x-www-form-urlencoded")]
    public class FeedbackController : Controller
    {
        
        private readonly IMailService _mailService;
        private readonly IFeedbackService  _feedbackService;
        public readonly IConfiguration _configuration;

        public FeedbackController( IMailService mailService, IFeedbackService feedbackService, IConfiguration configuration )
        {
            _mailService = mailService;
            _feedbackService = feedbackService;
            _configuration = configuration;
            
        }
        public ActionResult Index()
        {
            return View(new FeedbackVM());
        }
        [HttpPost]
        public async Task<JsonResult> JsonPostQuestionSendAsync([FromForm] FeedbackVM questionVM)
        {


            var email = new Email { Name = questionVM.Email };
            if (ModelState.IsValid/*&&homeVM.Email.Name!=null*/)
            {
                
                var sendMessageResponse = await _mailService.SendMessage(
                    email,
                    _configuration.GetSection("EmailSettings:EmailName").Value,
                    "Question",
                    "Thanks for your question!"
                );

                if (sendMessageResponse.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK)
                {
                    await _mailService.SendMessage(
                        new Email { Name = _configuration.GetSection("EmailSettings:EmailName").Value },
                        _configuration.GetSection("EmailSettings:EmailName").Value,
                        $"Message from {questionVM.Name}({questionVM.Email})",
                        questionVM.Message);

                    var SaveEmailResponse = await _feedbackService.SaveEmail(email);
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
            return Json(new BaseResponse<List<string>>
            {
                Data = invalidFields,
                Description = $"Model state is invalid",
                StatusCode = Yukidzaki_Domain.Enums.StatusCode.ModelStateIsInvalid
            });
        }
    }
}
