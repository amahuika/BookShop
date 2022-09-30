using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace BookShop.Models
{
    public class Company 
    {

      
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public int? PostalCode { get; set; }
        public int? PhoneNumber { get; set; }

    }
}
