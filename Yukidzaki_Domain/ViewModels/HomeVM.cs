using Yukidzaki_Domain.Models;


namespace Yukidzaki_Domain.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Token> Tokens { get; set; }
        

        public Email? Email { get; set; }
    }
}
