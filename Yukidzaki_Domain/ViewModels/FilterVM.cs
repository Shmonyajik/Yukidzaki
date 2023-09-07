using Yukidzaki_Domain.Models;

namespace Yukidzaki_Domain.ViewModels;

public class FilterVM
{
    public IEnumerable<Token> Tokens { get; set; }

    public int? tokensCount { get; set; }

    
}
