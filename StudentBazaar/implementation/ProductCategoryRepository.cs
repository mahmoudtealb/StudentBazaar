

namespace StudentBazaar.Web.implementation;
public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
{
    private readonly ApplicationDbContext _context;
    public ProductCategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(ProductCategory productCategory)
    {
        _context.ProductCategories.Update(productCategory);
    }
}