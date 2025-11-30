
using StudentBazaar.Web.Models.ViewModels;

namespace StudentBazaar.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // ======================
        // Register (GET)
        // ======================
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel
            {
                Universities = await _context.Universities
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = u.UniversityName
                    })
                    .ToListAsync(),
                Colleges = new List<SelectListItem>()
            };

            return View(model); // => Views/Account/Register.cshtml
        }

        // ======================
        // Register (POST)
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Universities = await _context.Universities
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = u.UniversityName
                    })
                    .ToListAsync();

                model.Colleges = await _context.Colleges
                    .Where(c => c.UniversityId == model.UniversityId)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.CollegeName
                    })
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
                // أي حد بيسجل من الـ UI العام يبقى Student بس
                await _userManager.AddToRoleAsync(user, "Student");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Product");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            // إعادة تحميل الـ dropdowns
            model.Universities = await _context.Universities
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.UniversityName
                })
                .ToListAsync();

            model.Colleges = await _context.Colleges
                .Where(c => c.UniversityId == model.UniversityId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CollegeName
                })
                .ToListAsync();

            return View(model);
        }

        // ======================
        // Login (GET)
        // ======================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        // ======================
        // Login (POST)
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);

                // مؤقتًا كله يروح على المنتجات
                return RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }

        // ======================
        // Logout
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ======================
        // Get Colleges (AJAX)
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
