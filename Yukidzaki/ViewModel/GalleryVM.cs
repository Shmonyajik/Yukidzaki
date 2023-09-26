using Yukidzaki.Models;

namespace Yukidzaki.ViewModel
{
    public class GalleryVM
    {
        public IEnumerable<Token> Tokens { get; set; }
        public IEnumerable<SeasonCollection> SeasonCollections { get; set; }
        public IEnumerable<Filter> Filters { get; set; }
        public IEnumerable<TokensFilters> TokensFilters { get; set; }








    }
}
