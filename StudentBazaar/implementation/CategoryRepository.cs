
using System.ComponentModel;

namespace StudentBazaar.Web.Repositories
{
    public class CategoryRepository : GenericRepository<CategoryAttribute>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateCategoryAttribute(CategoryAttribute categoryAttribute)
        {
           _context.UpdateCategoryAttribute(categoryAttribute);
        }
    }
}
