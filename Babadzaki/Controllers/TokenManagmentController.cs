using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Text;

namespace Babadzaki.Controllers
{
    public class TokenManagmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TokenManagmentController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        //ApplicationDbContext _context = new ApplicationDbContext();
        public IActionResult Index()
        {
            
            var tokens = _context.Tokens.Include(t => t.SeasonCollection);
            ViewBag.Count = tokens.Count();
            return View(tokens);
        }

        public IActionResult Details(int? id)
        {
            if(id==null||id==0)
            {
                return NotFound();
            }
            var token = _context.Tokens.Include(u=>u.SeasonCollection).FirstOrDefault(t=>t.Id==id);
            
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
            var token = _context.Tokens.Include(u => u.SeasonCollection).FirstOrDefault(u => u.Id == id);
            
            if (token == null)
            {
                return NotFound();
            }
            

            return View(token);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var token = _context.Tokens.Find(id);
            if (token == null)
            {
                return NotFound();
            }

            string filePath =_webHostEnvironment.WebRootPath+ WebConstants.ImagePath + token.Image;

            if(!string.IsNullOrEmpty(filePath)&& System.IO.File.Exists(filePath))
            {
               System.IO.File.Delete(filePath);
            }

            _context.Tokens.Remove(token);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


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

                if (tokenVM.token.Id!=0)
                {
                    //Update
                    var tokenFromDb = _context.Tokens.AsNoTracking().FirstOrDefault(t => t.Id == tokenVM.token.Id);//AsNo
                    //!!!!AsNoTracking для явного указания не отслеживать tokenFromDb для EFCore,
                    //!!!!так как tokenFromDB и tokenVM.token указывают на один объект в БД
                    if (files.Count>0)
                    {
                        string upload = webRootPath + WebConstants.ImagePath;// Babadzaki\wwwroot\ + img\token\
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName).ToLower();

                        var oldFile = Path.Combine(upload, tokenFromDb.Image==null?"": tokenFromDb.Image);

                        if(System.IO.File.Exists(oldFile))
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

      
    }
}

