namespace BookShop.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }


        public void Save();

    }
}
