using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Yukidzaki.Models
{
    public class Filter  
    {
        [Key]
        [ValidateNever]
        public int Id { get; set; }
        public string Name { get; set; }
       
        public virtual ICollection<TokensFilters> TokensFilters { get; set; }// навигационное свойство

        public Filter()
        {
            TokensFilters = new List<TokensFilters>();

        }
    }
}
