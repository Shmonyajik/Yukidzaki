using Babadzaki.Data;
using Babadzaki.ViewModel;
using Babadzaki_Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var filters = _context.TokensFilters.Include(f => f.Filter);
            ViewBag.Count = filters.Count();
            return View(filters);
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
        /// <summary>
        /// ///////////////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Upsert(int? id)//
        {

            FilterVM filterVM = new FilterVM
            {
                token = new Token(),
                seasonCollectionDropDown = _context.SeasonCollections.Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                })
            };


            if (id == null)
            {
                return View(tokenVM);
            }
            try
            {
                tokenVM.token = _context.Tokens.First(t => t.Id == id);
            }
            catch//Добавить конкретное исключение
            {

                return NotFound();
            }

            return View(tokenVM);
        }
        // Update/Create
        [HttpPost]
        public IActionResult Upsert(TokenVM tokenVM)//TODO: сделать кнопку для удаления картинки в обновлении
        {

            //using (StreamWriter w = new StreamWriter("C:\\Users\\shmon\\source\\repos\\Shmonyajik\\Babadzaki\\Babadzaki\\imageValidateLog.txt", false, Encoding.GetEncoding(1251)))
            //{
            //    string imageState = ModelState.FirstOrDefault(x => x.Key == "Item").Value.ToString();
            //    w.WriteLine(DateTime.Now.ToString());
            //    w.Write(imageState);

            //}
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;// загруженые файлы
                string webRootPath = _webHostEnvironment.WebRootPath;//абсолютный путь к каталогу

                if (tokenVM.token.Id != 0)
                {
                    //Update
                    var tokenFromDb = _context.Tokens.AsNoTracking().FirstOrDefault(t => t.Id == tokenVM.token.Id);//AsNo
                    //!!!!AsNoTracking для явного указания не отслеживать tokenFromDb для EFCore,
                    //!!!!так как tokenFromDB и tokenVM.token указывают на один объект в БД
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebConstants.ImagePath;// Babadzaki\wwwroot\ + img\token\
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName).ToLower();

                        var oldFile = Path.Combine(upload, tokenFromDb.Image == null ? "" : tokenFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (FileStream fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        tokenVM.token.Image = fileName + extension;

                    }
                    else
                    {
                        tokenVM.token.Image = tokenFromDb.Image;
                    }
                    _context.Tokens.Update(tokenVM.token);


                }
                else
                {
                    //Create
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebConstants.ImagePath;// Babadzaki\wwwroot\ + img\token\
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName).ToLower();

                        using (FileStream fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        tokenVM.token.Image = fileName + extension;

                    }
                    _context.Tokens.Add(tokenVM.token);
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            tokenVM.seasonCollectionDropDown = _context.SeasonCollections.Select(item => new SelectListItem//Подумать надо ли это
            {
                Text = item.Name,
                Value = item.Id.ToString()
            });
            return View(tokenVM);
        }
    }
}
