using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Models.ViewModels
{
    public class ProductVM
    {



        public Product product { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }

    }
}
