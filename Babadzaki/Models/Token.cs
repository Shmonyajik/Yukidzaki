using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Babadzaki.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(typeof(decimal), "0,0", "100000,9999999", ErrorMessage = "Наименьшая стоимость - 0 рублей, в качестве разделителя дробной и целой части используется запятая")]
        public decimal Price { get; set; }
        [DisplayName("Collection")]
        [Range(0, Int32.MaxValue, ErrorMessage ="This Collection is not exist")]
        public int? SeasonCollectionId { get; set; }
        [ValidateNever]//костыль?
        public SeasonCollection? SeasonCollection { get; set; }

        

    }
}
