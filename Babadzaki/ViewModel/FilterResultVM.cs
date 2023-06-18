using Babadzaki.Models;

namespace Babadzaki.ViewModel
{
    public class FilterResultVM
    {
        public IEnumerable<Token> Tokens { get; set; }

        public int? tokensCount { get; set; }
    }
}
