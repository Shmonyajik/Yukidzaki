﻿using Babadzaki.Data;
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
        public async Task<IActionResult> IndexAsync()
        {

            GalleryVM galleryVM = new GalleryVM
            {
                Tokens = _context.Tokens.Include(u => u.SeasonCollection),
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

            if (tokensFilters is null || tokensFilters.Count() == 0)
                return PartialView("_TokenCardGallery", _context.Tokens.ToList());

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
                        tokens = _context.Tokens.Where(t=>t.edition.ToString() == $"Yukidzaki #{searchQuery}").ToList();
                    //tokens = _context.Tokens.Where(t => t.edition.Substring(t.edition.IndexOf('#') + 1, t.edition.Length) == searchQuery).ToList();
                    else
                    {
                        //int index = tokens[0].edition.IndexOf('#') + 1;
                        //string sub = tokens[0].edition.Substring(tokens[0].edition.IndexOf('#') + 1);
                        //bool IsTrue = tokens[0].edition.Substring(tokens[0].edition.IndexOf('#') + 1) == searchQuery;

                        //tokens = tokens.Where(t => t.edition.ToString().Substring(t.edition.IndexOf('#') + 1) == searchQuery).ToList();
                    }
                    


                    return PartialView("_TokenCardGallery", tokens); 
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
