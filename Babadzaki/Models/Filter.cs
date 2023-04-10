using System.ComponentModel.DataAnnotations;

namespace Babadzaki.Models
{
    public class Filter  
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
       
        public virtual ICollection<TokensFilters> TokensFilters { get; set; }// навигационное свойство

        public Filter()
        {
            TokensFilters = new List<TokensFilters>();

        }
    }
}
