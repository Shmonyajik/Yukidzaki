using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Range(typeof(decimal), "0,0","99999,99999" , ErrorMessage = "The lowest value is 0.00 ETH, as a decimal and partially used comma separator")]
        public decimal Price { get; set; }
        
        
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

        [ValidateNever]//костыль?
        [ForeignKey("SeasonCollectionId")]
        public virtual SeasonCollection? SeasonCollection { get; set; }//virtual для ленивой загрузки(навигационное свойство)

        //[StringLength(maximumLength: 255, MinimumLength = 3, ErrorMessage = "Maximum number of characters 255")]

        public virtual ICollection<TokensFilters> TokensFilters { get; set; }// навигационное свойство

        public Token()
        {
            TokensFilters = new List<TokensFilters>();
        }



    }
}
