

namespace StudentBazaar.Web.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // ✅ Get all entities (with optional filter and include)
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeWord = null);

        // ✅ Get first or default entity
        Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            string? includeWord = null);

        // ✅ Add new entity
        Task AddAsync(T entity);

        // ✅ Remove one entity
        void Remove(T entity);

        // ✅ Remove multiple entities
        void RemoveRange(IEnumerable<T> entities);

        // ✅ Save changes
        Task SaveAsync();
    }
}
