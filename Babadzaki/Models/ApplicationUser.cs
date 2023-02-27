using Microsoft.AspNetCore.Identity;

namespace Babadzaki.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
