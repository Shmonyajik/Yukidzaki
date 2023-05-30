using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Babadzaki.Models
{
    public class TokensFilters /*: IEquatable<TokensFilters?> *///IEqualityComparer<Product>
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

        //public override bool Equals(object? obj)
        //{
        //    return Equals(obj as TokensFilters);
        //}

        //public bool Equals(TokensFilters? other)
        //{
        //    bool isTrue = other is not null &&
        //           Filter.Name == other.Filter.Name &&
        //           Value == other.Value;
        //    return isTrue;
        //}

        //public override int GetHashCode()
        //{
        //    return HashCode.Combine(Filter.Name, Value);
        //}
    }

    public class TokensFiltersComparer : IEqualityComparer<TokensFilters>
    {

        public bool Equals(TokensFilters x, TokensFilters y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Filter.Name == y.Filter.Name &&
                   x.Value == y.Value; 
        }

        public int GetHashCode(TokensFilters tokensFilters)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(tokensFilters, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashTokensFiltersName = tokensFilters.Filter.Name == null ? 0 : tokensFilters.Filter.Name.GetHashCode();

            //Get hash code for the Code field.
            int hashTokensFiltersCode = tokensFilters.Value.GetHashCode();

            //Calculate the hash code for the product.
            return hashTokensFiltersName ^ hashTokensFiltersCode;
        }
    
    }
}
