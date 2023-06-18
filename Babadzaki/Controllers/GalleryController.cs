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
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> IndexAsync()
        {
            Response.Cookies.Append("LastTokenIndex", "0");
            GalleryVM galleryVM = new GalleryVM
            {
                SeasonCollections = await _context.SeasonCollections.ToListAsync(),
                TokensFilters = _context.TokensFilters.Include(f => f.Filter).ToList().Distinct(new TokensFiltersComparer()),
                Filters = await _context.Filters.ToListAsync()

               
            };

            
            return View(galleryVM);
        }
        
        [HttpGet]
        public async Task<ActionResult> GetTokensByCollectionAsync(int collectionId)
        {
            var tokens = await _context.Tokens.Where(t => t.SeasonCollectionId == collectionId).ToListAsync();
            return new JsonResult(tokens);
            
        }
        
        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] IEnumerable<TokensFilters> tokensFilters)
        {
            _logger.LogWarning("Filter");
            
            FilterResultVM filterResultVM = null;
            //int number;

            if (tokensFilters is null || tokensFilters.Count() == 0)
            {
                var tokens = _context.Tokens.ToList();
                if (Request.Cookies.TryGetValue("LastTokenIndex", out string lastTokenIndexCookie))
                {
                    if (int.TryParse(lastTokenIndexCookie, out int lastTokenIndex))
                    {
                        try
                        {
                            filterResultVM = new FilterResultVM { Tokens = tokens.GetRange(lastTokenIndex, int.Parse(WebConstants.PageOfTokensSize)), tokensCount = tokens.Count() };
                            Response.Cookies.Append("LastTokenIndex", int.Parse(WebConstants.PageOfTokensSize) + lastTokenIndex.ToString());
                        }
                        catch (ArgumentException)
                        {
                            filterResultVM = new FilterResultVM { Tokens = tokens, tokensCount = tokens.Count() };
                            Response.Cookies.Append("LastTokenIndex", tokens.Count.ToString());

                        }
                                                    

                    }
                }
                else
                {
                    filterResultVM = new FilterResultVM { Tokens = tokens, tokensCount = tokens.Count() };
                }
                return PartialView("_TokenCardGallery", filterResultVM);
            }

            if (ModelState.IsValid)
                {
                    List<Token> tokens = null;
                    string searchQuery = null;
                    
                    foreach (var filter in tokensFilters)
                    {
                        if (filter.FilterId == 0)
                        {
                            searchQuery = filter.Value;
                            continue;
                        }
                        if (tokens == null)
                            tokens = _context.Tokens.Where(t => t.TokensFilters.FirstOrDefault(tf => tf.Value == filter.Value && tf.Filter.Id == filter.FilterId)!=null).Include(tf => tf.TokensFilters).ThenInclude(f => f.Filter).ToList();
                        else
                            tokens = tokens.Where(t => t.TokensFilters.FirstOrDefault(tf => tf.Value == filter.Value && tf.Filter.Id == filter.FilterId) != null).ToList();

                    }
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        if (tokens == null)
                            tokens = _context.Tokens.Where(t => t.edition.ToString() == searchQuery).ToList();
                        //tokens = _context.Tokens.Where(t => t.edition.Substring(t.edition.IndexOf('#') + 1, t.edition.Length) == searchQuery).ToList();
                        else
                        {
                            //int index = tokens[0].edition.IndexOf('#') + 1;
                            //string sub = tokens[0].edition.Substring(tokens[0].edition.IndexOf('#') + 1);
                            //bool IsTrue = tokens[0].edition.Substring(tokens[0].edition.IndexOf('#') + 1) == searchQuery;

                            tokens = tokens.Where(t => t.edition.ToString() == searchQuery).ToList();
                        }
                    }


                    if (Request.Cookies.TryGetValue("LastTokenIndex", out string lastTokenIndexCookie))
                    {
                        if (int.TryParse(lastTokenIndexCookie, out int lastTokenIndex))
                        {
                            try
                            {
                                filterResultVM = new FilterResultVM { Tokens = tokens.GetRange(lastTokenIndex, int.Parse(WebConstants.PageOfTokensSize)), tokensCount = tokens.Count() };
                                Response.Cookies.Append("LastTokenIndex", int.Parse(WebConstants.PageOfTokensSize) + lastTokenIndex.ToString());
                            }
                            catch (ArgumentException)
                            {
                                filterResultVM = new FilterResultVM { Tokens = tokens, tokensCount = tokens.Count() };
                                Response.Cookies.Append("LastTokenIndex", tokens.Count.ToString());

                            }

                        }
                    }
                    else
                    {
                        filterResultVM = new FilterResultVM { Tokens = tokens, tokensCount = tokens.Count() };
                    }



                    return PartialView("_TokenCardGallery", filterResultVM);
                    
                }
            return new JsonResult(NotFound());

        }
        [HttpGet]
        public async Task<IActionResult> PartialViewAction(int id, bool isJson)
        {
            var response = await _context.Tokens.Include(u => u.SeasonCollection).Include(f => f.TokensFilters).ThenInclude(tf => tf.Filter).FirstOrDefaultAsync(t => t.Id == id);


            return PartialView("_ModalTokenDetails",  response);
        }


    }
}
