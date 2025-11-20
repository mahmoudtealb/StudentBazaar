
using StudentBazaar.Web.ViewModels;

namespace StudentBazaar.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGenericRepository<Product> _repo;
        private readonly IGenericRepository<ProductCategory> _categoryRepo;
        private readonly IGenericRepository<StudyYear> _studyYearRepo;

        public ProductController(
            IGenericRepository<Product> repo,
            IGenericRepository<ProductCategory> categoryRepo,
            IGenericRepository<StudyYear> studyYearRepo)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
            _studyYearRepo = studyYearRepo;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _repo.GetAllAsync(includeWord: "Category,StudyYear,Images,Listings,Ratings");
            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(p => p.Id == id, includeWord: "Category,StudyYear,Images,Listings,Ratings");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            var model = new ProductCreateViewModel
            {
                Categories = (await _categoryRepo.GetAllAsync())
                                .Select(c => new SelectListItem(c.CategoryName, c.Id.ToString())),
                StudyYears = (await _studyYearRepo.GetAllAsync())
                                .Select(s => new SelectListItem(s.YearName, s.Id.ToString()))
            };

            return View(model);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = (await _categoryRepo.GetAllAsync())
                                .Select(c => new SelectListItem(c.CategoryName, c.Id.ToString()));
                model.StudyYears = (await _studyYearRepo.GetAllAsync())
                                .Select(s => new SelectListItem(s.YearName, s.Id.ToString()));
                return View(model);
            }

            // إضافة المنتج
            var product = model.Product;
            await _repo.AddAsync(product);
            await _repo.SaveAsync();

            // رفع الصور
            if (model.Files != null && model.Files.Any())
            {
                foreach (var file in model.Files)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    product.Images.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = "/uploads/" + fileName,
                        IsMainImage = false
                    });
                }

                await _repo.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(p => p.Id == id, includeWord: "Images");
            if (existing == null)
                return NotFound();

            var model = new ProductCreateViewModel
            {
                Product = existing,
                Categories = (await _categoryRepo.GetAllAsync())
                                .Select(c => new SelectListItem(c.CategoryName  , c.Id.ToString())),
                StudyYears = (await _studyYearRepo.GetAllAsync())
                                .Select(s => new SelectListItem(s.YearName, s.Id.ToString()))
            };

            return View(model);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = (await _categoryRepo.GetAllAsync())
                                .Select(c => new SelectListItem(c.CategoryName, c.Id.ToString()));
                model.StudyYears = (await _studyYearRepo.GetAllAsync())
                                .Select(s => new SelectListItem(s.YearName, s.Id.ToString()));
                return View(model);
            }

            var existing = await _repo.GetFirstOrDefaultAsync(p => p.Id == id, includeWord: "Images");
            if (existing == null)
                return NotFound();

            existing.Name = model.Product.Name;
            existing.CategoryId = model.Product.CategoryId;
            existing.StudyYearId = model.Product.StudyYearId;
            existing.UpdatedAt = DateTime.Now;

            // رفع صور جديدة
            if (model.Files != null && model.Files.Any())
            {
                foreach (var file in model.Files)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    existing.Images.Add(new ProductImage
                    {
                        ProductId = existing.Id,
                        ImageUrl = "/uploads/" + fileName,
                        IsMainImage = false
                    });
                }
            }

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
                return NotFound();

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
