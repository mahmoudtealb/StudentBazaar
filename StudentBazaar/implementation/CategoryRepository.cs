using Microsoft.EntityFrameworkCore;
using StudentBazaar.Web.Models;
using System.ComponentModel;

namespace StudentBazaar.Web.Repositories
{
    public class CategoryRepository : GenericRepository<CategoryAttribute>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Update category
        public void UpdateCategoryAttribute(CategoryAttribute categoryAttribute)
        {
            _context.Update(categoryAttribute);
        }
    }
}
