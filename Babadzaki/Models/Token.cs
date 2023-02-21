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
        [Range(typeof(decimal), "0,0", "100000,9999999", ErrorMessage = "The lowest value is 0.00 ETH, as a decimal and partially used comma separator")]
        public decimal Price { get; set; }
        [StringLength(255, ErrorMessage = "Maximum number of characters 255")]
        #region
        //TODO: No store type was specified for the decimal property 'Price' on entity type 'Token'.
        //This will cause values to be silently truncated if they do not fit in the default precision and scale.
        //Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType',
        //specify precision and scale using 'HasPrecision', or configure a value 
        #endregion
        public string? Description { get; set; }
        [RegularExpression(@"[^s]+(.(?i)(jpg|png|gif|bmp))$", ErrorMessage = "Incorrect path")]
        public string? Image { get; set; }
        // [^s]+(.(?i)(jpg|png|gif|bmp))$

        [DisplayName("SeasonCollection Type")]
        [Range(0, Int32.MaxValue, ErrorMessage = "This Collection is not exist")]
        public int? SeasonCollectionId { get; set; }

        [ValidateNever]//костыль?
        [ForeignKey("SeasonCollectionId")]
        public virtual SeasonCollection? SeasonCollection { get; set; }//virtual для ленивой загрузки



    }
}
