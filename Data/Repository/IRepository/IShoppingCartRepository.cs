using BookShop.Models;

namespace BookShop.Data.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<Cart>
    {

      int IncrementCount(Cart cart, int count);

      int DecrementCount(Cart cart, int count);



    }
}
