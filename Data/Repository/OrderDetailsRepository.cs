using BookShop.Data.Repository.IRepository;
using BookShop.Models;

namespace BookShop.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {

        private ApplicationDbContext _db;


        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderDetails obj)
        {
            _db?.OrderDetails?.Update(obj);
        }


    }
}
