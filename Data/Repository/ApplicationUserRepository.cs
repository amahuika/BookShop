using BookShop.Data.Repository.IRepository;
using BookShop.Models;

namespace BookShop.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {

        private ApplicationDbContext _db;


        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


     
    }
}
