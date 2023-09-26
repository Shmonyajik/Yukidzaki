using Yukidzaki.Models;

namespace Yukidzaki.ViewModel
{
    public class FilterResultVM
    {
        public IEnumerable<Token> Tokens { get; set; }

        public int? tokensCount { get; set; }
    }
}
