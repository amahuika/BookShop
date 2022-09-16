

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
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 300)]
        public double Price { get; set; }

        [Required]
        [Range(1, 300)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 300)]
        public double Price100 { get; set; }



        public string ImageUrl { get; set;}


        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        [Required]
        public int CoverTypeId { get; set; }
        public CoverType CoverType { get; set; }

    }
}
