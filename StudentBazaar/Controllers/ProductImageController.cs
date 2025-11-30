using Microsoft.AspNetCore.Mvc;
using StudentBazaar.Web.Models;

namespace StudentBazaar.Web.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly IGenericRepository<ProductImage> _repo;
        private readonly IWebHostEnvironment _env;

        public ProductImageController(
            IGenericRepository<ProductImage> repo,
            IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        // GET: ProductImage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductImage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductImage entity, IFormFile File)
        {
            if (File == null || File.Length == 0)
            {
                ModelState.AddModelError("File", "You must upload an image.");
            }

            if (!ModelState.IsValid)
            {
                return View(entity);
            }

            // إنشاء مجلد الصور داخل wwwroot
            string uploadPath = Path.Combine(_env.WebRootPath, "images/products");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // إنشاء اسم فريد للصورة
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            // حفظ الصورة داخل wwwroot
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            // مسار الصورة
            entity.ImageUrl = "/images/products/" + fileName;
            entity.CreatedAt = DateTime.Now;

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ProductImage/Index
        public async Task<IActionResult> Index()
        {
            var images = await _repo.GetAllAsync(includeWord: "Product");
            return View(images);
        }
    }
}
