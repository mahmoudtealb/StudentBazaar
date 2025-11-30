using Microsoft.AspNetCore.Mvc;
using StudentBazaar.Web.Models;
using StudentBazaar.Web.Repositories;

namespace StudentBazaar.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenericRepository<Product> _productRepo;

        public HomeController(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        // الصفحة الرئيسية - Landing Page
        public async Task<IActionResult> Index()
        {
            // نجيب كل المنتجات مع الصور و الكاتيجوري
            var products = await _productRepo.GetAllAsync(
                includeWord: "Category,Images,Ratings"
            );

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
