

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1,300)]
        [Display(Name = "List Price")]

        public double ListPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 1-50")]

        public double Price { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 50-100")]

        public double Price50 { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Pricefor100+")]

        public double Price100 { get; set; }


        [ValidateNever]
        [Display(Name ="Image")]
        public string ImageUrl { get; set;}


        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }


        [Required]
        [Display(Name = "Cover Type")]
        public int CoverTypeId { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }

    }
}
