using Yukidzaki.Models;

namespace Yukidzaki.ViewModel
{
    public class JsonFilterResultVM
    {
        public IEnumerable<Token> Tokens { get; set; }

        public string partialViewHTML { get; set; }
    }
}
