using BookShop.Models;

namespace BookShop.Data.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {

        void Update(OrderHeader obj);

        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);


        void UpdatePaymentId(int id, string sessionId, string paymentIntentId);





    }
}
