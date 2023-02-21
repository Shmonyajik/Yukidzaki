using Babadzaki.Data;
using Babadzaki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Babadzaki.Controllers
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;


        public TokenController(ApplicationDbContext context)
        {
            _context = context;

        }
        //ApplicationDbContext _context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var timeService = HttpContext.RequestServices.GetRequiredService<ITimeService>();
            var tokens = _context.Tokens.Include(t => t.SeasonCollection);
            ViewBag.Count = tokens.Count();
            ViewBag.Time = timeService.GetTime();
            return View(tokens);
        }

        public IActionResult Details(int? id)
        {
            if(id==null||id==0)
            {
                return NotFound();
            }
            var token = _context.Tokens.Find(id);
            ViewBag.SeasonCollection = _context.SeasonCollections.Find(token.SeasonCollectionId);//TODO: подумать как получить название коллекции
            if(token==null)
            {
                return NotFound();
            }
                
            return View(token);
        }



        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//проверка на валидность токена(безопасность)
        public IActionResult Create(Token token)
        {
            if (ModelState.IsValid)
            {
                _context.Add(token);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(token);//Обратно в Create


        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var token = _context.Tokens.Find(id);
            if (token == null)
            {
                return NotFound();
            }

            return View(token);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Token token)
        {
            if (ModelState.IsValid)
            {
                _context.Tokens.Update(token);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(token);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var token = _context.Tokens.Find(id);
            if (token == null)
            {
                return NotFound();
            }

            return View(token);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var token = _context.Tokens.Find(id);
            if (token == null)
            {
                return NotFound();
            }
            _context.Tokens.Remove(token);
            _context.SaveChanges();
            return RedirectToAction("Index");


        }

    }
}

