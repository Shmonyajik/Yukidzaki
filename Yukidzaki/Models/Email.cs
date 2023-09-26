using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Yukidzaki.Models
{
    public class Email
    {
        [Key]
        [BindProperty]
        public int Id { get; set; }
        [Required]
        [BindProperty]
        [EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
        public string Name { get; set; }
}
}
