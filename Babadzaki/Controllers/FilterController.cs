using Babadzaki.Data;
using Babadzaki.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace Babadzaki.Controllers
{
    [Consumes("application/x-www-form-urlencoded")]
    public class FilterController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        
        public FilterController(ILogger<HomeController> logger, ApplicationDbContext context)
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
    }
}
