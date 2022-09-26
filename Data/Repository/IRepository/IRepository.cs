using System.Linq.Expressions;

namespace BookShop.Data.Repository.IRepository
{
    // Generic repository where t will be a class
    public interface IRepository<T> where T : class
    {

        // only common methods this is a generic repository 
        // Category
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(string? includeProperties = null);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);








    }
}
