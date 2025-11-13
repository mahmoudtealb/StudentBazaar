
namespace StudentBazaar.Web.Controllers
{
    public class UniversityController : Controller
    {
        private readonly IGenericRepository<University> _repo;

        public UniversityController(IGenericRepository<University> repo)
        {
            _repo = repo;
        }

        // GET: University
        public async Task<IActionResult> Index()
        {
            var universities = await _repo.GetAllAsync(includeWord: "Colleges,Users");
            return View(universities);
        }

        // GET: University/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(u => u.Id == id, includeWord: "Colleges,Users");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: University/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: University/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(University entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: University/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(u => u.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: University/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, University entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(u => u.Id == id);
            if (existing == null)
                return NotFound();

            existing.UniversityName = entity.UniversityName;
            existing.Location = entity.Location;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: University/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(u => u.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: University/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(u => u.Id == id);
            if (entity == null)
                return NotFound();

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
