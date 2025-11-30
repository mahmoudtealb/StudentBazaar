namespace StudentBazaar.Web.Repositories;

public interface IProductCategoryRepository : IGenericRepository<ProductCategory>
{
    void Update(ProductCategory productCategory);
}

