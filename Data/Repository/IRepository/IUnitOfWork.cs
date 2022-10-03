namespace BookShop.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }

        IApplicationUserRepository ApplicationUser { get; }
        IShoppingCart ShoppingCart { get; }





        public void Save();

    }
}
