using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentBazaar.Web.Models;
using StudentBazaar.Web.Repositories;

namespace StudentBazaar.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserRepository userRepo, UserManager<ApplicationUser> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                                          .Include(u => u.University)
                                          .Include(u => u.College)
                                          .ToListAsync();
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var entity = await _userManager.FindByIdAsync(id);

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
        public async Task<IActionResult> Create(ApplicationUser entity, string password, string role)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var result = await _userRepo.CreateAsync(entity, password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(entity);
            }

            if (!string.IsNullOrEmpty(role))
                await _userRepo.AddToRoleAsync(entity, role);

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var existing = await _userManager.FindByIdAsync(id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser entity)
        {
            if (!ModelState.IsValid)
                return View(entity);

            var existing = await _userManager.FindByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.FullName = entity.FullName;
            existing.Email = entity.Email;
            existing.UserName = entity.Email; // ضروري للـ Identity
            existing.PhoneNumber = entity.PhoneNumber;
            existing.Address = entity.Address;
            existing.UniversityId = entity.UniversityId;
            existing.CollegeId = entity.CollegeId;

            await _userRepo.UpdateAsync(existing);

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var entity = await _userManager.FindByIdAsync(id);
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);
            if (entity == null)
                return NotFound();

            await _userManager.DeleteAsync(entity);
            return RedirectToAction(nameof(Index));
        }
    }
}
