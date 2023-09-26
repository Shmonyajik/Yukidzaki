using Yukidzaki.Models;

namespace Yukidzaki.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Token> Tokens { get; set; }
        public IEnumerable<SeasonCollection> SeasonCollections { get; set; }

        public Email? Email { get; set; }
    }
}
