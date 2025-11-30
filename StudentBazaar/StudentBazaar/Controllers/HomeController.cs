using Microsoft.AspNetCore.Mvc;
using StudentBazaar.Web.Models;
using StudentBazaar.Web.Repositories;

namespace StudentBazaar.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<College> _collegeRepo;

        public HomeController(IGenericRepository<Product> productRepo, IGenericRepository<College> collegeRepo)
        {
            _productRepo = productRepo;
            _collegeRepo = collegeRepo;
        }

        // الصفحة الرئيسية - Landing Page
        public async Task<IActionResult> Index(string? q = null, int? collegeId = null)
        {
            var products = await _productRepo.GetAllAsync(
                includeWord: "Category,Images,Ratings,Owner"
            );

            if (!string.IsNullOrWhiteSpace(q))
            {
                var qLower = q.Trim();
                products = products.Where(p =>
                    p.Name.Contains(qLower, StringComparison.OrdinalIgnoreCase) ||
                    (p.Owner != null && p.Owner.College != null && p.Owner.College.CollegeName.Contains(qLower, StringComparison.OrdinalIgnoreCase))
                );
            }

            if (collegeId.HasValue)
                products = products.Where(p => p.Owner != null && p.Owner.CollegeId == collegeId.Value);

            var colleges = await _collegeRepo.GetAllAsync();
            ViewBag.Colleges = colleges;
            ViewBag.CurrentQuery = q;
            ViewBag.CurrentCollegeId = collegeId;

            return View(products);
        }

        [HttpGet]
        public Task<IActionResult> Search(string? q = null, int? collegeId = null)
        {
            return Index(q, collegeId);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
