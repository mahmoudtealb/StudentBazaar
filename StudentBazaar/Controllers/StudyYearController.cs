namespace StudentBazaar.Web.Controllers
{
    public class StudyYearController : Controller
    {
        private readonly IGenericRepository<StudyYear> _repo;

        public StudyYearController(IGenericRepository<StudyYear> repo)
        {
            _repo = repo;
        }

        // GET: StudyYear
        public async Task<IActionResult> Index()
        {
            var years = await _repo.GetAllAsync(includeWord: "Major,Products");
            return View(years);
        }

        // GET: StudyYear/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(s => s.Id == id, includeWord: "Major,Products");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: StudyYear/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudyYear/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudyYear entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: StudyYear/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(s => s.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: StudyYear/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudyYear entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(s => s.Id == id);
            if (existing == null)
                return NotFound();

            existing.YearName = entity.YearName;
            existing.MajorId = entity.MajorId;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: StudyYear/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: StudyYear/Delete/5
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
