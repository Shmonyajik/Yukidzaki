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
            //ViewBag.Model = _context.Tokens;
            return View(_context.Tokens);
        }
        // TODO: изменить вывод коллекции(не идентификаор а название)
        public IActionResult Details(int tokenId)
        {
            
            return View(_context.Tokens.First(t => t.Id == tokenId));
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

        public IActionResult Delete(int id)
        {
            _context.Remove(id);
            return View();
        }
    }
}
