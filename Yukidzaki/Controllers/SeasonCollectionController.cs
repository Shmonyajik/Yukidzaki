using Babadzaki.Data;
using Microsoft.AspNetCore.Mvc;

namespace Babadzaki.Controllers
{
    public class SeasonCollectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeasonCollectionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id)
        {
            if (id == null)// ||id==0
                return NotFound();
            var tokens = _context.Tokens.Where(x => x.SeasonCollectionId == id).ToList();
            ViewBag.Count = tokens.Count;
            ViewBag.SeasonCollection = _context.SeasonCollections.Find(id).Name;

            return View(tokens);
        }
    }
}
