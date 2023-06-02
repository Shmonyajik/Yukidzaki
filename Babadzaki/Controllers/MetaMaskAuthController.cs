using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Babadzaki.Controllers
{
    public class MetaMaskAuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateOneTimeCode()
        {
            byte[] randomBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            var oneTimeCode = BitConverter.ToString(randomBytes).Replace("-", string.Empty);
            // Store the one-time code for the user as needed
            return Ok(oneTimeCode);
        }
    }
}
