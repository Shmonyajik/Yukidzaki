using Babadzaki_Serivces.Implementations;
using Babadzaki_Serivces.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nethereum.ABI.EIP712;

namespace Babadzaki.Controllers
{
    public class RefactoringTestController : Controller
    {
        public readonly ITokenService _tokenService;

        public RefactoringTestController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var response = await _tokenService.GetToken();
            if (response.StatusCode == Babadzaki_Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
    }
}
