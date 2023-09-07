using Babadzaki_Domain.Models;

namespace Babadzaki_Domain.ViewModels
{
    public class GalleryVM
    {
        public IEnumerable<Token> Tokens { get; set; }
        public IEnumerable<SeasonCollection> SeasonCollections { get; set; }
        public IEnumerable<Filter> Filters { get; set; }
        public IEnumerable<TokensFilters> TokensFilters { get; set; }

        public Dictionary<int, int> TokensWithAttributeCount { get; set; }








    }
}
