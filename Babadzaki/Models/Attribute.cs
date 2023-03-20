using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Babadzaki.Models
{
    public class Attribute
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<TokensAttributes> TokensAttributes { get; set; }// навигационное свойство

        public bool IsChecked { get; set; } = false;

        public Attribute()
        {
            TokensAttributes = new List<TokensAttributes>();
        }
        [Range(0, Int32.MaxValue, ErrorMessage = "This Collection is not exist")]
        public int? FilterId { get; set; }

        [ValidateNever]//костыль?
        [ForeignKey("FilterId")]
        public virtual Filter? Filter { get; set; }//virtual для ленивой загрузки(навигационное свойство)

    }
}
