using Babadzaki_Domain.Models;

namespace Babadzaki_Domain.ViewModels;

public class FilterVM
{
    public IEnumerable<Token> Tokens { get; set; }

    public int? tokensCount { get; set; }
}
