using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StudentBazaar.Web.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        // Get all entities (optionally filtered and including related data)
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate, string? includeWord)
        {
            IQueryable<T> query = dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            if (!string.IsNullOrEmpty(includeWord))
                query = query.Include(includeWord);

            return query.ToList();
        }

        // Get the first matching entity or return null if not found
        public T? GetFirstOrDefault(Expression<Func<T, bool>> predicate, string? includeWord)
        {
            IQueryable<T> query = dbSet;

            if (!string.IsNullOrEmpty(includeWord))
                query = query.Include(includeWord);

            return query.FirstOrDefault(predicate);
        }

        // Add a new entity to the database
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        // Remove a single entity
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        // Remove multiple entities at once
        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
