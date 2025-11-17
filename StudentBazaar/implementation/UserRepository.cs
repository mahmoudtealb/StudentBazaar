namespace StudentBazaar.Web.implementation;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    // Update entity
    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    // Get user by email
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
                             .Include(u => u.University)
                             .Include(u => u.College)
                             .FirstOrDefaultAsync(u => u.Email == email);
    }

    // Override to support multiple includes via string
    public new async Task<IEnumerable<User>> GetAllAsync(string includeWord = null)
    {
        return await base.GetAllAsync(null, includeWord);
    }

    public new async Task<User?> GetFirstOrDefaultAsync(Expression<Func<User, bool>> filter, string includeWord = null)
    {
        return await base.GetFirstOrDefaultAsync(filter, includeWord);
    }
}