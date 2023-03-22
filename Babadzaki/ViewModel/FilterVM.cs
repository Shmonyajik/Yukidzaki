using Babadzaki.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Babadzaki.ViewModel
{
    public class FilterVM
    {
        public Filter filter { get; set; }

        public IEnumerable<SelectListItem>? filtersDropDown { get; set; }
    }
}
