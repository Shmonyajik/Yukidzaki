using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_Domain.ViewModels
{
    public class TokenVM
    {
        [DisplayName("Edition")]
        [Required]
        public int edition { get; set; }
        #region
        [DisplayName("Description")]
        #endregion
        [StringLength(maximumLength: 255, MinimumLength = 3, ErrorMessage = "Maximum number of characters 255")]
        public string? Description { get; set; }
        [DisplayName("Image")]
        [RegularExpression(@"([^\\s]+(\\.(?i)(jpg|png|gif|bmp))$)", ErrorMessage = "Incorrect path")]
        public string? Image { get; set; }
        

        [DisplayName("Season Collection")]
        [Range(0, Int32.MaxValue, ErrorMessage = "This Collection is not exist")]
        public int? SeasonCollectionId { get; set; }
    }
}
