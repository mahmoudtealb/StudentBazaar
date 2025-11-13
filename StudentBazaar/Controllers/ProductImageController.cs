namespace StudentBazaar.Web.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly IGenericRepository<ProductImage> _repo;

        public ProductImageController(IGenericRepository<ProductImage> repo)
        {
            _repo = repo;
        }

        // GET: ProductImage
        public async Task<IActionResult> Index()
        {
            var images = await _repo.GetAllAsync(includeWord: "Product");
            return View(images);
        }

        // GET: ProductImage/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(p => p.Id == id, includeWord: "Product");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: ProductImage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductImage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductImage entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductImage/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: ProductImage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductImage entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
                return NotFound();

            existing.ImageUrl = entity.ImageUrl;
            existing.IsMainImage = entity.IsMainImage;
            existing.ProductId = entity.ProductId;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductImage/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: ProductImage/Delete/5
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
