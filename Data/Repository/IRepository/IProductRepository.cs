using BookShop.Models;

namespace BookShop.Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {

        void Update(Product obj);


    }
}
