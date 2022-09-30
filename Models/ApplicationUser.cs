using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace BookShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
       



    }
}
