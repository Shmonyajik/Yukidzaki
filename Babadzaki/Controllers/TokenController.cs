using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Babadzaki.Controllers
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TokenController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        // Update/Create
        [HttpGet]
        public IActionResult Upsert(int? id)//
        {
            
            TokenVM tokenVM = new TokenVM
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
                tokenVM.token = _context.Tokens.First(t=>t.Id==id);
            }
            catch//Добавить конкретное исключение
            {
                
                return NotFound();
            }
            
            return View(tokenVM);
        }
        // Update/Create
        [HttpPost]
        public IActionResult Upsert(TokenVM tokenVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;// загруженые файлы
                string webRootPath = _webHostEnvironment.WebRootPath;//абсолютный путь к каталогу

                if (tokenVM.token.Id!=0)
                {
                    //Update
                    if(files.Count>0)
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
                return RedirectToAction("Index");
            }
            return View(new TokenVM {
                    token = new Token(),
                    seasonCollectionDropDown = _context.SeasonCollections.Select(item => new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    })
            });
        }
        #region create/edit
        //[HttpGet]
        //public IActionResult Create()
        //{

        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]//проверка на валидность токена(безопасность)
        //public IActionResult Create(Token token)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(token);
        //        _context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(token);//Обратно в Create


        //}

        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var token = _context.Tokens.Find(id);
        //    if (token == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(token);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Token token)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Tokens.Update(token);
        //        _context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(token);

        //}
        #endregion

        //private string SaveFiles(IFormFileCollection files, string webRootPath)
        //{
        //    string upload = webRootPath + WebConstants.ImagePath;// Babadzaki\wwwroot\ + img\token\
        //    string fileName = Guid.NewGuid().ToString();
        //    string extension = Path.GetExtension(files[0].FileName).ToLower();

        //    using (FileStream fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
        //    {
        //        files[0].CopyTo(fileStream);
        //    }
        //    return fileName + extension;
        //}
    }
}

