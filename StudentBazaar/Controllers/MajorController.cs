namespace StudentBazaar.Web.Controllers
{
    public class MajorController : Controller
    {
        private readonly IGenericRepository<Major> _repo;

        public MajorController(IGenericRepository<Major> repo)
        {
            _repo = repo;
        }

        // GET: Major
        public async Task<IActionResult> Index()
        {
            var majors = await _repo.GetAllAsync(includeWord: "College,StudyYears");
            return View(majors);
        }

        // GET: Major/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(m => m.Id == id, includeWord: "College,StudyYears");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: Major/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Major/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Major entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Major/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(m => m.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: Major/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Major entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(m => m.Id == id);
            if (existing == null)
                return NotFound();

            existing.MajorName = entity.MajorName;
            existing.CollegeId = entity.CollegeId;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Major/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: Major/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
                return NotFound();

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
