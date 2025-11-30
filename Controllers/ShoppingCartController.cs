namespace StudentBazaar.Web.Controllers
{
    public class ShoppingCartItemController : Controller
    {
        private readonly IGenericRepository<ShoppingCartItem> _repo;

        public ShoppingCartItemController(IGenericRepository<ShoppingCartItem> repo)
        {
            _repo = repo;
        }

        // GET: ShoppingCartItem
        public async Task<IActionResult> Index()
        {
            var items = await _repo.GetAllAsync(includeWord: "User,Listing");
            return View(items);
        }

        // GET: ShoppingCartItem/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(s => s.Id == id, includeWord: "User,Listing");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: ShoppingCartItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShoppingCartItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShoppingCartItem entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ShoppingCartItem/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(s => s.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: ShoppingCartItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShoppingCartItem entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(s => s.Id == id);
            if (existing == null)
                return NotFound();

            existing.UserId = entity.UserId;
            existing.ListingId = entity.ListingId;
            existing.Quantity = entity.Quantity;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ShoppingCartItem/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: ShoppingCartItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
                return NotFound();

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
