

namespace StudentBazaar.Web.Controllers
{
    public class CollegeController : Controller
    {
        private readonly IGenericRepository<College> _repo;

        public CollegeController(IGenericRepository<College> repo)
        {
            _repo = repo;
        }

        // GET: College
        public async Task<IActionResult> Index()
        {
            var colleges = await _repo.GetAllAsync(includeWord: "University");
            return View(colleges);
        }

        // GET: College/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(c => c.Id == id, includeWord: "University,Users,Majors");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: College/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: College/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(College entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: College/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(c => c.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: College/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, College entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(c => c.Id == id);
            if (existing == null)
                return NotFound();

            existing.CollegeName = entity.CollegeName;
            existing.UniversityId = entity.UniversityId;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: College/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(c => c.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: College/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(c => c.Id == id);
            if (entity == null)
                return NotFound();

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
