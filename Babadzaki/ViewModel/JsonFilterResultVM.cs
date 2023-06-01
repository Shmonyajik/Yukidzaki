using Babadzaki.Models;

namespace Babadzaki.ViewModel
{
    public class JsonFilterResultVM
    {
        public IEnumerable<Token> Tokens { get; set; }

        public string partialViewHTML { get; set; }
    }
}
