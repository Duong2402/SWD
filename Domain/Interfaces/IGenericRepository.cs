using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FilterAll(Expression<Func<T, bool>> predicate, string includeProperties = "");
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int page, int size);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);

        Task DeleteByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetDetailById(object id, string includeProperties = "");
    }
}
