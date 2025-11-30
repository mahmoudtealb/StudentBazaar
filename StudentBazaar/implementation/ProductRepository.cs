using StudentBazaar.Web.Repositories;

namespace StudentBazaar.Web.implementation;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .Include(p => p.Category) // اختياري لو عايز تجيب بيانات الكاتيجوري كمان
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }


    public void Update(Product product)
    {
        _context.Products.Update(product);
    }
}
