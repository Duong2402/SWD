using Application.Common.Pagination;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T>(DbContext context) : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int page, int size)
        {
            var totalCount = await _context.Set<T>().CountAsync();
            var items = await _context.Set<T>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task DeleteByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task<IEnumerable<T>> FilterAll(Expression<Func<T, bool>> predicate,
                                                    string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = PageMethod.IncludeClass(query, includeProperties);
            }

            query = query.Where(predicate);

            return await query.ToListAsync();
        }

        public async Task<T> GetDetailById(object id, string includeProperties = "")
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null) return null; 

            IQueryable<T> query = _dbSet.Where(e => EF.Property<object>(e, "Id") == id);
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = PageMethod.IncludeClass(query, includeProperties);
            }

            return await query.FirstOrDefaultAsync();
        }

        // Example usage: _repository.GetPagedAsync(1, 10, x => x.Name.Contains("Memaybeo"), e => e.OrderByDescending(x => x.DateCreated)); 
        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
           int page = 1,
           int size = 10,
           Expression<Func<T, bool>>? filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalCount = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var items = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
