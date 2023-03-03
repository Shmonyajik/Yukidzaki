using System.ComponentModel.DataAnnotations;

namespace Babadzaki.Models
{
    public class Email
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
        public string Name { get; set; }
}
}
