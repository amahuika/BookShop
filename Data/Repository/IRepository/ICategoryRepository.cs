using BookShop.Models;

namespace BookShop.Data.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {

        void Update(CoverType obj);


    }
}
