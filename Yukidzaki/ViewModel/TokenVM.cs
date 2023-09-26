using Yukidzaki.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Yukidzaki.ViewModel
{
    public class TokenVM
    {
        public Token token { get; set; }
        
        public IEnumerable<SelectListItem>? seasonCollectionDropDown { get; set; }

        public IEnumerable<SelectListItem>? filtersDropDown { get; set; }


    }
}
