namespace BookShop.Models.ViewModels
{
    public class ShoppingCartVM
    {

        public IEnumerable<Cart> CartList { get; set; }

        public double TotalPrice { get; set; }

        public OrderHeader OrderHeader { get; set; }

    }
}
