

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Yukidzaki_Services.Interfaces;
using Yukidzaki_Domain.ViewModels;
using Babadzaki_Services;
using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Services.Implementations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Yukidzaki.Controllers
{
    [Consumes("application/x-www-form-urlencoded")]
    public class FeedbackController : Controller
    {
        
        private readonly IMailService _mailService;
        private readonly IFeedbackService  _feedbackService;

        public FeedbackController( IMailService mailService, IFeedbackService feedbackService )
        {
            _mailService = mailService;
            _feedbackService = feedbackService;
            
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
                    WebConstants.EmailFrom,
                    "Question",
                    "Thanks for your question!"
                );

                if (sendMessageResponse.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK)
                {
                    await _mailService.SendMessage(
                        new Email { Name = WebConstants.EmailFrom},
                        WebConstants.EmailFrom,
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
