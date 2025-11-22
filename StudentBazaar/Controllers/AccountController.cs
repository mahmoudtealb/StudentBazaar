using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentBazaar.Web.Models;
using StudentBazaar.Web.Models.ViewModels;

namespace StudentBazaar.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // ======================
        // 🔹 Register (GET)
        // ======================
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel
            {
                Universities = await _context.Universities
                                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UniversityName })
                                    .ToListAsync(),
                Colleges = new List<SelectListItem>() // Start empty, load by AJAX
            };
            return View(model);
        }

        // ======================
        // 🔹 Register (POST)
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Universities = await _context.Universities
                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UniversityName })
                    .ToListAsync();
                model.Colleges = await _context.Colleges
                    .Where(c => c.UniversityId == model.UniversityId)
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CollegeName })
                    .ToListAsync();
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                UniversityId = model.UniversityId,
                CollegeId = model.CollegeId,
                Address = model.Address
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign selected role (Student)
                var roleToAssign = model.Role ?? "Student"; // Default to Student if not specified
                await _userManager.AddToRoleAsync(user, roleToAssign);

                // Sign in user
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Product");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            model.Universities = await _context.Universities
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UniversityName })
                .ToListAsync();

            model.Colleges = await _context.Colleges
                .Where(c => c.UniversityId == model.UniversityId)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CollegeName })
                .ToListAsync();

            return View(model);
        }

        // ======================
        // 🔹 Login (GET)
        // ======================
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // ======================
        // 🔹 Login (POST)
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Get the logged-in user
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Get user's roles
                    var userRoles = await _userManager.GetRolesAsync(user);
                    
                    // Validate that user has the selected role
                    if (!string.IsNullOrEmpty(model.Role) && !userRoles.Contains(model.Role))
                    {
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError("", $"You are not registered as a {model.Role}. Please select the correct role or register with that role.");
                        return View(model);
                    }
                }

                // Redirect to Product page as required
                return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        // ======================
        // 🔹 Logout
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ======================
        // 🔹 Get Colleges by University (AJAX)
        // ======================
        [HttpGet]
        public async Task<JsonResult> GetColleges(int universityId)
        {
            var colleges = await _context.Colleges
                .Where(c => c.UniversityId == universityId)
                .Select(c => new { c.Id, c.CollegeName })
                .ToListAsync();

            return Json(colleges);
        }
    }
}
