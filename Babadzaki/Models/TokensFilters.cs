using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Babadzaki.Models
{
    public class TokensFilters : IEquatable<TokensFilters?>
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

        public override bool Equals(object? obj)
        {
            return Equals(obj as TokensFilters);
        }

        public bool Equals(TokensFilters? other)
        {
            return other is not null &&
                   other.Value.Equals(this.Value) && other.Filter.Name.Equals(this.Filter.Name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Filter.Name, Value);
        }
    }
}
