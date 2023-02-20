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
            ViewBag.Time = timeService.GetTime();
            return View(_context.Tokens.Include(t => t.SeasonCollection));
        }
        // TODO: изменить вывод коллекции(не идентификаор а название)
        public IActionResult Details(int? Id)
        {
            return View(_context.Tokens.First(t => t.Id == Id));

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
            if(ModelState.IsValid)
            {
                _context.Add(token);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(token);//Обратно в Create
            
            
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int? Id)
        {
            if (Id != null)
            {
                Token token = _context.Tokens.FirstOrDefault(p => p.Id == Id);
                if (token != null)
                    return View(token);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(int? Id)
        {
            if (Id != null)
            {
                Token token = _context.Tokens.FirstOrDefault(p => p.Id == Id);
                if (token != null)
                {
                    _context.Tokens.Remove(token);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
