using BookShop.Models;

namespace BookShop.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {

        void Update(Category obj);



     


    }
}
