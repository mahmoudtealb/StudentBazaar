
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace StudentBazaar.Web.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe);
        Task SignOutAsync();
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    }
}
