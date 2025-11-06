using System.Linq.Expressions;

namespace StudentBazaar.Web.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // Retrieve all entities (optionally filtered and including related data)
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate, string? includeWord);

        // Retrieve the first entity that matches the condition, or null if not found
        T? GetFirstOrDefault(Expression<Func<T, bool>> predicate, string? includeWord);

        // Add a new entity to the context
        void Add(T entity);

        // Remove a single entity from the context
        void Remove(T entity);

        // Remove multiple entities from the context
        void RemoveRange(IEnumerable<T> entities);
    }
}
