using Yukidzaki_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yukidzaki_Domain.ViewModels
{
    public class FilterPageVM
    {
        public IEnumerable<TokensFilters> tokensFilters { get; set; } 

        public int page { get; set; }
    }
}
