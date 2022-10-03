﻿using BookShop.Data.Repository.IRepository;
using BookShop.Models;

namespace BookShop.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCart ShoppingCart { get; private set; }




        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);



        }




        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
