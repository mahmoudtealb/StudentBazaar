namespace StudentBazaar.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IGenericRepository<User> _repo;

        public UserController(IGenericRepository<User> repo)
        {
            _repo = repo;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _repo.GetAllAsync(includeWord: "University,College,ListingsPosted,OrdersPlaced,RatingsGiven,ShipmentsHandled,ShoppingCartItems");
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(u => u.Id == id, includeWord: "University,College,ListingsPosted,OrdersPlaced,RatingsGiven,ShipmentsHandled,ShoppingCartItems");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _repo.GetFirstOrDefaultAsync(u => u.Id == id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _repo.GetFirstOrDefaultAsync(u => u.Id == id);
            if (existing == null)
                return NotFound();

            existing.FullName = entity.FullName;
            existing.Email = entity.Email;
            existing.Phone = entity.Phone;
            existing.PasswordHash = entity.PasswordHash;
            existing.Role = entity.Role;
            existing.Address = entity.Address;
            existing.UniversityId = entity.UniversityId;
            existing.CollegeId = entity.CollegeId;
            existing.UpdatedAt = DateTime.Now;

            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetFirstOrDefaultAsync(u => u.Id == id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: User/Delete/5
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
