using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Yukidzaki.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}

