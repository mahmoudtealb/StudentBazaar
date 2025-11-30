namespace StudentBazaar.Web.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    void Update(Product product);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
}
