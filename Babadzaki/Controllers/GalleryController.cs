using Babadzaki.Data;
using Babadzaki.Models;
using Babadzaki.Utility;
using Babadzaki.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

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
            return View(_context.Tokens.Include(u => u.SeasonCollection));
        }
        [HttpGet]
        public async Task<JsonResult> GetTokensByCollectionAsync(int collectionId)
        {
            var tokens = await _context.Tokens.Where(t => t.SeasonCollectionId == collectionId).ToListAsync();
            return new JsonResult(tokens);
            
        }
        [HttpGet]
        public  JsonResult Filter([FromBody]GalleryVM filterVM)
        {
            _logger.LogWarning("Hyu");


            if (ModelState.IsValid)
            {
                var tokens = new List<Token>();

                foreach (var filter in filterVM.Filters)
                {
                    if(filter.IsChecked)
                    {
                        foreach (var attribute in filter.Attributes)
                        {
                            if(attribute.IsChecked)
                            {
                                tokens.AddRange(_context.Tokens.Where(t => t.TokensAttributes.First(a => a.AttributeId == attribute.Id) != null)
                                    .Include(c => c.SeasonCollection));

                            }
                        }
                    }
                    
                }

                
                return new JsonResult(tokens);
            }
            return new JsonResult(NotFound());

        }


    }
}
