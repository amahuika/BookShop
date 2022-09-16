using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookShop.Models;

namespace BookShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BookShop.Models.Category>? Category { get; set; }
        public DbSet<BookShop.Models.CoverType>? CoverType { get; set; }
        public DbSet<BookShop.Models.Product>? Product { get; set; }

    }
}