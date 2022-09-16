using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Category
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Display(Name = "Display Order")]
        [Range(1,100, ErrorMessage = "Display order must be between 1 and 100")]
        [Required]
        public int DisplayOrder { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;




    }
}
