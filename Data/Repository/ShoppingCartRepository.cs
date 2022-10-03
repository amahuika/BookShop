using BookShop.Data.Repository.IRepository;
using BookShop.Models;

namespace BookShop.Data.Repository
{
    public class ShoppingCartRepository : Repository<Cart> , IShoppingCart
    {

        private ApplicationDbContext _db;


        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecrementCount(Cart cart, int count)
        {
            cart.Count -= count;
            return cart.Count;
        }

        public int IncrementCount(Cart cart, int count)
        {
            cart.Count += count;
            return count;
        }
    }
}
