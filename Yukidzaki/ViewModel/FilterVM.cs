using Yukidzaki.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Yukidzaki.ViewModel
{
    public class FilterVM
    {
        public Filter filter { get; set; }

        public IEnumerable<SelectListItem>? filtersDropDown { get; set; }
    }
}
