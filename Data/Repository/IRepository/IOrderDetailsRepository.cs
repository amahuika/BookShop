using BookShop.Models;

namespace BookShop.Data.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {

        void Update(OrderDetails obj);



     


    }
}
