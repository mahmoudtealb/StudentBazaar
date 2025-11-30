
namespace StudentBazaar.Web.Implementation
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Rating rating)
        {
            _context.Ratings.Update(rating);
        }
    }
}
