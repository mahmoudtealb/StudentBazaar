namespace StudentBazaar.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGenericRepository<Product> _repo;

        public ProductController(IGenericRepository<Product> repo)
        {
            _repo = repo;
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
                return NotFound();

            existing.Name = entity.Name;
            existing.CategoryId = entity.CategoryId;
            existing.StudyYearId = entity.StudyYearId;
            existing.UpdatedAt = DateTime.Now;

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
