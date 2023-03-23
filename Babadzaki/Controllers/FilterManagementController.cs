using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.ViewModel;
using Babadzaki_Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace Babadzaki.Controllers
{
    public class FilterManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FilterManagementController> _logger;
       
        public FilterManagementController(ApplicationDbContext context, ILogger<FilterManagementController> logger)
        {
            _context = context;
            _logger = logger;
            
        }
        // GET: FilterManagmentController
        public ActionResult Index()
        {
            var filters = _context.Filters.ToList();
            ViewBag.Count = filters.Count();
            
            return View(filters);

        }

        // GET: FilterManagmentController/Details/5
        public ActionResult Details(int? Id)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var filter = _context.Filters.Include(f=>f.TokensFilters).FirstOrDefault(f=>f.Id==Id);

            if (filter == null)
            {
                return NotFound();
            }


            return View(filter);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int? id)
        {
            var filter = _context.Tokens.Find(id);
            if (filter == null)
            {
                return NotFound();
            }


            _context.Tokens.Remove(filter);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
      
        [HttpGet]
        public IActionResult Upsert(int? Id)//
        {

            var filter = new Filter();


            if (Id == null)
            {
                return View(filter);
            }
            try
            {
                filter = _context.Filters.First(t => t.Id == Id);
            }
            catch//Добавить конкретное исключение
            {

                return NotFound();
            }

            return View(filter);
        }
        // Update/Create
        [HttpPost]
        public IActionResult Upsert(Filter filter)
        {
            #region Old
            //using (StreamWriter w = new StreamWriter("C:\\Users\\shmon\\source\\repos\\Shmonyajik\\Babadzaki\\Babadzaki\\imageValidateLog.txt", false, Encoding.GetEncoding(1251)))
            //{
            //    string imageState = ModelState.FirstOrDefault(x => x.Key == "Item").Value.ToString();
            //    w.WriteLine(DateTime.Now.ToString());
            //    w.Write(imageState);

            //}
            //if (ModelState.IsValid)
            //{
            //    var files = HttpContext.Request.Form.Files;// загруженые файлы
            //    string webRootPath = _webHostEnvironment.WebRootPath;//абсолютный путь к каталогу

            //    if (filterVM.filter.Id != 0)
            //    {
            //        Update
            //        var tokenFromDb = _context.Tokens.AsNoTracking().FirstOrDefault(t => t.Id == tokenVM.token.Id);//AsNo
            //        !!!!AsNoTracking для явного указания не отслеживать tokenFromDb для EFCore,
            //        !!!!так как tokenFromDB и tokenVM.token указывают на один объект в БД
            //        if (files.Count > 0)
            //        {
            //            string upload = webRootPath + WebConstants.ImagePath;// Babadzaki\wwwroot\ + img\token\
            //            string fileName = Guid.NewGuid().ToString();
            //            string extension = Path.GetExtension(files[0].FileName).ToLower();

            //            var oldFile = Path.Combine(upload, tokenFromDb.Image == null ? "" : tokenFromDb.Image);

            //            if (System.IO.File.Exists(oldFile))
            //            {
            //                System.IO.File.Delete(oldFile);
            //            }

            //            using (FileStream fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            //            {
            //                files[0].CopyTo(fileStream);
            //            }
            //            tokenVM.token.Image = fileName + extension;

            //        }
            //        else
            //        {
            //            tokenVM.token.Image = tokenFromDb.Image;
            //        }
            //        _context.Tokens.Update(tokenVM.token);


            //    }
            //    else
            //    {
            //        Create
            //        if (files.Count > 0)
            //        {
            //            string upload = webRootPath + WebConstants.ImagePath;// Babadzaki\wwwroot\ + img\token\
            //            string fileName = Guid.NewGuid().ToString();
            //            string extension = Path.GetExtension(files[0].FileName).ToLower();

            //            using (FileStream fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            //            {
            //                files[0].CopyTo(fileStream);
            //            }
            //            tokenVM.token.Image = fileName + extension;

            //        }
            //        _context.Tokens.Add(tokenVM.token);
            //    }
            //    _context.SaveChanges();
            //    return RedirectToAction(nameof(Index));

            //}
            //tokenVM.seasonCollectionDropDown = _context.SeasonCollections.Select(item => new SelectListItem//Подумать надо ли это
            //{
            //    Text = item.Name,
            //    Value = item.Id.ToString()
            //});
            #endregion
            if (ModelState.IsValid)
            {   //Update
                if (filter.Id != 0)
                {
                    if (filter != null)
                    {
                        _context.Filters.Update(filter);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                //Create
                else
                {
                    if (filter != null)
                    {
                        _context.Filters.Add(filter);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                    
                   
            }
               
            return View(filter);
        }
    }
}
