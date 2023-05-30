using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Babadzaki_Utility;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NuGet.Protocol;
using Microsoft.Extensions.ObjectPool;
using AutoMapper;
using NuGet.Common;

namespace Babadzaki.Controllers
{
    
    public class TokenManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<TokenManagementController> _logger;
        private readonly IMapper _mapper;

        public TokenManagementController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<TokenManagementController> logger, IMapper mapper)
        { 

            _logger = logger;
            _mapper = mapper;
            try
            {
                _mapper.ConfigurationProvider.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
            }
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
            var token = _context.Tokens.Include(u=>u.SeasonCollection).Include(f=>f.TokensFilters).ThenInclude(tf=>tf.Filter).FirstOrDefault(t=>t.Id==id);
            
            if(token==null)
            {
                return NotFound();
            }
                
            return View(token);
        }



       
        public IActionResult Delete(int id)
        {
            if ( id == 0)
            {
                return NotFound();
            }
            var token = _context.Tokens.Include(u => u.SeasonCollection).Include(f => f.TokensFilters).ThenInclude(tf => tf.Filter).FirstOrDefault(t => t.Id == id);//TOO : узнать почему сначала include

            if (token == null)
            {
                return NotFound();
            }
            

            return View(token);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var token = _context.Tokens.Find(id);
            

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
                token = new Models.Token(),
                seasonCollectionDropDown = _context.SeasonCollections.Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
                filtersDropDown = _context.Filters.Select(item=>new SelectListItem
                { Text = item.Name,
                  Value = item.Id.ToString()
                })
            };

            
            if (id == null)
            {
                return View(tokenVM);
            }
            try
            {
                tokenVM.token = _context.Tokens.Include(t=>t.TokensFilters).ThenInclude(tf=>tf.Filter).First(t=>t.Id==id);
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

        [HttpGet]
        public IActionResult LoadJsonToken()
        {
            
             return PartialView ("_LoadJsonToken");
        }
        [HttpPost]
        public async Task<JsonResult> LoadJsonTokenPost()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        _logger.LogError("Один или более файлов пусты!");
                    }

                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var fileString = await reader.ReadToEndAsync();
                        JsonToken jsonToken = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonToken>(fileString);
                        SeasonCollection seasonCollection = null;
                        Models.Token token = _mapper.Map<Models.Token>(jsonToken);
                        if (_context.Tokens.FirstOrDefault(t => t.dna == token.dna) != null)
                        {
                            reader.Close();
                            continue;
                        }

                        foreach (var attr in jsonToken.attributes)
                        {
                            if(attr.trait_type=="Season")
                            {
                                try
                                {
                                    token.SeasonCollection = _context.SeasonCollections.First(sc => sc.Name == attr.value);
                                }
                                catch (Exception)
                                {
                                    token.SeasonCollection = new SeasonCollection { Name = attr.value };
                                }
                                continue;
                            }
                            //try
                            //{
                            //    token.TokensFilters.Add(_context.TokensFilters.First(tf => tf.Value == attr.value && tf.Filter.Name == attr.trait_type));
                            //}
                            //catch (Exception)
                            //{
                                token.TokensFilters.Add(new TokensFilters{
                                    Value = attr.value,
                                    Filter = _context.Filters.FirstOrDefault(f => f.Name == attr.trait_type) ?? new Filter
                                    {
                                        Name = attr.trait_type
                                    }
                                });
                                

                            //}

                        }
                        _logger.LogInformation($"{token.Image}: {token.GetHashCode()}\n");

                        _context.Tokens.Add(token);
                        _context.SaveChanges();
                    }
                    
                }
                

            }
            return new JsonResult(Ok());
        }

        //[HttpPost]
        //public IActionResult PartialTokenDetails([FromBody] Models.Token _token)
        //{
        //    var token = _token;
        //    // Additional processing logic

        //    return PartialView("_ModalTokenDetails", token);
        //}

    }
}

