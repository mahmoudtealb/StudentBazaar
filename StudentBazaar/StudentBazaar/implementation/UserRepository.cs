

namespace StudentBazaar.Web.implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // ==========================
        // Create a new user with password
        // ==========================
        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        // ==========================
        // Update user details
        // ==========================
        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        // ==========================
        // Get user by email
        // ==========================
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _userManager.Users
                .Include(u => u.University)
                .Include(u => u.College)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // ==========================
        // Sign in user with password
        // ==========================
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        }

        // ==========================
        // Sign out current user
        // ==========================
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        // ==========================
        // Get roles of a user
        // ==========================
        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        // ==========================
        // Add user to a role
        // ==========================
        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        // ==========================
        // Get all users with related University and College
        // ==========================
        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users
                .Include(u => u.University)
                .Include(u => u.College)
                .ToListAsync();
        }

        // ==========================
        // Get first user matching a filter
        // ==========================
        public async Task<ApplicationUser?> GetFirstOrDefaultAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            return await _userManager.Users
                .Include(u => u.University)
                .Include(u => u.College)
                .FirstOrDefaultAsync(filter);
        }
    }
}
