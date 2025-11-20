
namespace StudentBazaar.Web.Implementation
{
    public class UniversityRepository : GenericRepository<University>, IUniversityRepository
    {
        private readonly ApplicationDbContext _context;

        public UniversityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Update entity
        public void Update(University university)
        {
            _context.Universities.Update(university);
        }

        // Override to support multiple includes via string
        public new async Task<IEnumerable<University>> GetAllAsync(string includeWord = null)
        {
            return await base.GetAllAsync(null, includeWord);
        }

        public new async Task<University?> GetFirstOrDefaultAsync(Expression<Func<University, bool>> filter, string includeWord = null)
        {
            return await base.GetFirstOrDefaultAsync(filter, includeWord);
        }
    }
}
