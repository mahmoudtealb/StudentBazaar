/*using StudentBazaar.Web.repositories;

namespace StudentBazaar.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGenericRepository<Product> _productRepo;

        public ProductController(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            return View(products);
        }
    }
}
*/
