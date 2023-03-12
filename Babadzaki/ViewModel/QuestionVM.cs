using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Babadzaki.ViewModel
{
    public class QuestionVM
    {
        [Required]
        public string? Name { get; set; }
        [EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
        [Required]
        public string Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [StringLength(maximumLength: 255, MinimumLength = 10, ErrorMessage = "Maximum number of characters 255, Minimal number of characters 10 ")]
        [Required]
        public string Message { get; set; }
    }
}
