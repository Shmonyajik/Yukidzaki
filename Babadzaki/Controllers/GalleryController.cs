using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki_Utility;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using System.IO;
using System.Text;

namespace Babadzaki.Controllers
{
    [Consumes("application/json")]
    public class GalleryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        
        public GalleryController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            GalleryVM galleryVM = new GalleryVM
            {
                Tokens = _context.Tokens.Include(u => u.SeasonCollection).ToList(),
                SeasonCollections = _context.SeasonCollections.ToList(),
                Filters = _context.Filters.ToList().Distinct(),
                TokensFilters = _context.TokensFilters.Include(f => f.Filter).ToList()
            };
            return View(galleryVM);
        }
        [HttpGet]
        public async Task<JsonResult> GetTokensByCollectionAsync(int collectionId)
        {
            var tokens = await _context.Tokens.Where(t => t.SeasonCollectionId == collectionId).ToListAsync();
            return new JsonResult(tokens);
            
        }
        
        [HttpGet]
        public JsonResult Filter([FromBody] IEnumerable<TokensFilters> tokensFilters)
        {
            _logger.LogWarning("Filter");
            //GalleryVM galleryVM = new GalleryVM();
            //try
            //{
            //    using (FileStream streamReader = new FileStream(@"json.json", FileMode.Open, FileAccess.Read))
            //    {
            //        galleryVM = System.Text.Json.JsonSerializer.Deserialize<GalleryVM>(streamReader);
            //        streamReader.Close();
            //    }
            //    _logger.LogInformation("Vnature Chetko");
            //}
            //catch (Exception)
            //{

            //    _logger.LogError("Vse Huinya");
            //}

            if (tokensFilters == null)
                _logger.LogCritical("NE NASHEL!!!!");
            if (ModelState.IsValid)
                {
                    var tokens = new List<Token>();

                    foreach (var filter in tokensFilters)
                    {
                        tokens.AddRange(_context.Tokens.Where(t=>t.TokensFilters.FirstOrDefault(tf=>tf.Value==filter.Value)!=null).Distinct());
                    
                    }

                    if (tokens.Count > 0)
                    {
                        //tokens =  tokens.Union(tokens).ToList();//TODO подумать как удалять дубликаты
                        return new JsonResult(tokens);
                    }
                }
            return new JsonResult(NotFound());

        }
        [HttpGet]
        public IActionResult PartialViewAction()
        {
            //var deserializedModel = JsonConvert.DeserializeObject<ModalTokenDetatilsVM>(_token);
            return PartialView("_ModalTokenDetails");
        }


    }
}
