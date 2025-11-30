

namespace StudentBazaar.Web.implementation;
public class MajorRepository : GenericRepository<Major>, IMajorRepository
{
    private readonly ApplicationDbContext _context;
    public MajorRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Major major)
    {
        _context.Majors.Update(major);
    }
}
