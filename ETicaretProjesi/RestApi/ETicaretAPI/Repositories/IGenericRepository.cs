using System.Linq.Expressions;
using ETicaretAPI.Wrappers; // for PagedResponse if needed, or just return List/Tuple

namespace ETicaretAPI.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Generic Pagination
        Task<(List<T> Data, int TotalRecords)> GetAllPagedAsync(
            int pageNumber, 
            int pageSize, 
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params string[] includeProperties);
    }
}