using Babadzaki_Domain.Models;


namespace Babadzaki_Domain.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Token> Tokens { get; set; }
        

        public Email? Email { get; set; }
    }
}
