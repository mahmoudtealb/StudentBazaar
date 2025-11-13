namespace StudentBazaar.Web.Controllers
{
    public class ListingController : Controller
    {
        private readonly IGenericRepository<Listing> _repo;

        public ListingController(IGenericRepository<Listing> repo)
        {
            _repo = repo;
        }

        // GET: Listing
        public async Task<IActionResult> Index()
        {
            var listings = await _repo.GetAllAsync(includeWord: "Product,Seller,Orders,ShoppingCartItems");
            return View(listings);
        }

        // GET: Listing/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(l => l.Id == id, includeWord: "Product,Seller,Orders,ShoppingCartItems");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: Listing/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Listing entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Listing/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(l => l.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: Listing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Listing entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(l => l.Id == id);
            if (existing == null)
                return NotFound();

            existing.Price = entity.Price;
            existing.Condition = entity.Condition;
            existing.Description = entity.Description;
            existing.Discount = entity.Discount;
            existing.Status = entity.Status;
            existing.PostingDate = entity.PostingDate;
            existing.ProductId = entity.ProductId;
            existing.SellerId = entity.SellerId;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Listing/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(l => l.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: Listing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(l => l.Id == id);
            if (entity == null)
                return NotFound();

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
