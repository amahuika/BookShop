using BookShop.Data.Repository.IRepository;
using BookShop.Models;

namespace BookShop.Data.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {

        private ApplicationDbContext _db;


        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Company obj)
        {
            _db?.Company.Update(obj);
        }



    }
}
