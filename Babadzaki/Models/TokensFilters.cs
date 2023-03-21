using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Babadzaki.Models
{
    public class TokensFilters
    {
        [Key]
        public int Id { get; set; }
        public int TokenId { get; set; }
        [ValidateNever]//костыль?
        [ForeignKey("TokenId")]
        public virtual Token Token { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "This Collection is not exist")]
        public int FilterId { get; set; }
        [ValidateNever]//костыль?
        [ForeignKey("FilterId")]
        public virtual Filter Filter { get; set; }

        [StringLength(maximumLength: 255, ErrorMessage = "Maximum number of characters 255")]
        public string Value { get; set; }

        public bool IsChecked { get; set; }//Сделать мапинг(убрать из бд => добавить в контроллер)



    }
}
