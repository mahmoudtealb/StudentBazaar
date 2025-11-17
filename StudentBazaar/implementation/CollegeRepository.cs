

namespace StudentBazaar.Web.Implementation;

// Now ICollegeRepository is recognized
public class CollegeRepository : GenericRepository<College>, ICollegeRepository
{
    private readonly ApplicationDbContext _context;
    public CollegeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(College college)
    {
        _context.Colleges.Update(college);
    }
}