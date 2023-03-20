using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Babadzaki.Models
{
    public class TokensAttributes
    {
        [Key]
        public int Id { get; set; }
        public int TokenId { get; set; }
        [ValidateNever]//костыль?
        [ForeignKey("SeasonCollectionId")]
        public virtual Token Token { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "This Collection is not exist")]
        public int AttributeId { get; set; }
        [ValidateNever]//костыль?
        [ForeignKey("AttributeId")]
        public virtual Attribute Attribute { get; set; }



    }
}
