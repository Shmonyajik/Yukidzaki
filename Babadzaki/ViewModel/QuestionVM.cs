using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Babadzaki.ViewModel
{
    public class QuestionVM
    {
        [BindProperty]
        public string? Name { get; set; }
        [EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
        [Required]
        [BindProperty]
        public string Email { get; set; }
        [Phone]
        [BindProperty]
        public string? Phone { get; set; }
        [StringLength(maximumLength: 255, MinimumLength = 10, ErrorMessage = "Maximum number of characters 255, Minimal number of characters 10 ")]
        [Required]
        [BindProperty]
        public string Message { get; set; }
    }
}
