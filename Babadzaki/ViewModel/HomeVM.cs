using Babadzaki.Models;

namespace Babadzaki.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Token> Tokens { get; set; }
        public IEnumerable<SeasonCollection> SeasonCollections { get; set; }
    }
}
