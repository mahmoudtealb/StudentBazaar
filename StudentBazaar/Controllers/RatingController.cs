namespace StudentBazaar.Web.Controllers
{
    public class RatingController : Controller
    {
        private readonly IGenericRepository<Rating> _repo;

        public RatingController(IGenericRepository<Rating> repo)
        {
            _repo = repo;
        }

        // GET: Rating
        public async Task<IActionResult> Index()
        {
            var ratings = await _repo.GetAllAsync(includeWord: "User,Product");
            return View(ratings);
        }

        // GET: Rating/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(r => r.Id == id, includeWord: "User,Product");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: Rating/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rating/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rating entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Rating/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(r => r.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: Rating/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rating entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(r => r.Id == id);
            if (existing == null)
                return NotFound();

            existing.UserId = entity.UserId;
            existing.ProductId = entity.ProductId;
            existing.Stars = entity.Stars;
            existing.Comment = entity.Comment;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Rating/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(r => r.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: Rating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(r => r.Id == id);
            if (entity == null)
                return NotFound();

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
