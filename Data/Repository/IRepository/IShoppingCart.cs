using BookShop.Models;

namespace BookShop.Data.Repository.IRepository
{
    public interface IShoppingCart : IRepository<Cart>
    {

      int IncrementCount(Cart cart, int count);

      int DecrementCount(Cart cart, int count);



    }
}
