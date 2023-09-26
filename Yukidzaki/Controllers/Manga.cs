using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Yukidzaki.Controllers
{
    public class Manga : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}

