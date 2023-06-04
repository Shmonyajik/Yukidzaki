using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Babadzaki.Models
{
    public class Token : IEquatable<Token?>
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        [Required]
        public int edition { get; set; }
        #region
        //TODO: No store type was specified for the decimal property 'Price' on entity type 'Token'.
        //This will cause values to be silently truncated if they do not fit in the default precision and scale.
        //Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType',
        //specify precision and scale using 'HasPrecision', or configure a value 
        #endregion
        [StringLength(maximumLength: 255, MinimumLength = 3, ErrorMessage = "Maximum number of characters 255")]
        public string? Description { get; set; }
        //TODO: регулярное выражение скорректировать
        [RegularExpression(@"([^\\s]+(\\.(?i)(jpg|png|gif|bmp))$)", ErrorMessage = "Incorrect path")]
        //[RegularExpression(@"^.*\.(jpg|gif|jpeg|png|bmp)$", ErrorMessage = "Некорректный адрес")]
        public string? Image { get; set; }
        // [^s]+(.(?i)(jpg|png|gif|bmp))$

        [DisplayName("Season Collection")]
        [Range(0, Int32.MaxValue, ErrorMessage = "This Collection is not exist")]
        public int? SeasonCollectionId { get; set; }

        [JsonIgnore]
        [ValidateNever]//костыль?
        [ForeignKey("SeasonCollectionId")]
        public virtual SeasonCollection? SeasonCollection { get; set; }//virtual для ленивой загрузки(навигационное свойство)

        //[StringLength(maximumLength: 255, MinimumLength = 3, ErrorMessage = "Maximum number of characters 255")]
        [JsonIgnore]
        public virtual ICollection<TokensFilters> TokensFilters { get; set; }// навигационное свойство
        
        public Token()
        {
            TokensFilters = new List<TokensFilters>();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Token);
        }

        public bool Equals(Token? other)
        {
            return other is not null &&
                   edition == other.edition;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(edition);
        }
    }
}
