namespace BookShop.Models.ViewModels
{
    public class OrderVM
    {

        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }


    }
}
