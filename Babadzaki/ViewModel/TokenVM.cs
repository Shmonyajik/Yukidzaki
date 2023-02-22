using Babadzaki.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Babadzaki.ViewModel
{
    public class TokenVM
    {
        public Token token { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> seasonCollectionDropDown { get; set; }
    }
}
