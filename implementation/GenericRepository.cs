using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StudentBazaar.Web.Data;

namespace StudentBazaar.Web.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        // ============================
        //        GET ALL
        // ============================
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeWord = null)
        {
            IQueryable<T> query = dbSet.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            // FIX: Proper include handling (NO dynamic expression)
            if (!string.IsNullOrEmpty(includeWord))
            {
                var includes = includeWord.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var includeProp in includes)
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();
        }

        // ============================
        //   GET FIRST OR DEFAULT
        // ============================
        public async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            string? includeWord = null)
        {
            IQueryable<T> query = dbSet.Where(predicate);

            if (!string.IsNullOrEmpty(includeWord))
            {
                var includes = includeWord.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var includeProp in includes)
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        // ============================
        //           ADD
        // ============================
        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        // ============================
        //         REMOVE
        // ============================
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        // ============================
        //         SAVE
        // ============================
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
