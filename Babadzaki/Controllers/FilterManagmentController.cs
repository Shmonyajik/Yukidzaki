using Babadzaki.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Babadzaki.Controllers
{
    public class FilterManagmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FilterManagmentController(ApplicationDbContext context)
        {
            _context= context;
        }
        // GET: FilterManagmentController
        public ActionResult Index()
        {
            return View(/*_context.Attributes.Include(a=>a.Filter)*/);
        }

        // GET: FilterManagmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FilterManagmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FilterManagmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FilterManagmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FilterManagmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FilterManagmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FilterManagmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
