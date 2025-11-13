namespace StudentBazaar.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IGenericRepository<Order> _repo;

        public OrderController(IGenericRepository<Order> repo)
        {
            _repo = repo;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var orders = await _repo.GetAllAsync(includeWord: "Listing,Buyer,Shipment");
            return View(orders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(o => o.Id == id, includeWord: "Listing,Buyer,Shipment");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(o => o.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(o => o.Id == id);
            if (existing == null)
                return NotFound();

            existing.ListingId = entity.ListingId;
            existing.BuyerId = entity.BuyerId;
            existing.OrderDate = entity.OrderDate;
            existing.Status = entity.Status;
            existing.PaymentMethod = entity.PaymentMethod;
            existing.TotalAmount = entity.TotalAmount;
            existing.SiteCommission = entity.SiteCommission;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(o => o.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(o => o.Id == id);
            if (entity == null)
                return NotFound();

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
