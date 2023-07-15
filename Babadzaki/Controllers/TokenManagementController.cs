using Babadzaki_Domain.Responses;
using Babadzaki_Serivces.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Babadzaki.Controllers
{
    public class TokenManagementController : Controller
    {
        private readonly ITokenService _tokenService;
        public TokenManagementController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpGet]
        public IActionResult LoadToken()
        {
            return View("LoadToken");
        }
        public async Task<JsonResult> LoadJsonTokenPost()
        {
            var response = new BaseResponse<bool>();
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    response = await _tokenService.LoadTokens(files);
                }
                else
                {
                    response.Description = "No files uploaded";
                    response.StatusCode = Babadzaki_Domain.Enums.StatusCode.JsonReaderError;
                }

            }
            else
            {
                var invalidFields = ModelState.Values
              .Where(v => v.ValidationState == ModelValidationState.Invalid)
              .SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage)
              .ToList();
                return Json(new BaseResponse<List<string>>
                {
                    Data = invalidFields,
                    Description = $"Model state is invalid",
                    StatusCode = Babadzaki_Domain.Enums.StatusCode.ModelStateIsInvalid
                });
            }

            return Json(response);
        }
    }
}
