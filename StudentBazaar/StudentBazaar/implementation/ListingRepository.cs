

namespace StudentBazaar.Web.implementation;

public class ListingRepository : GenericRepository<Listing>, IListingRepository
{
    private readonly ApplicationDbContext _context;
    public ListingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Listing listing)
    {
        _context.Listings.Update(listing);
    }

    public async Task<IEnumerable<Listing>> GetByUserAsync(int userId)
    {
        return await _context.Listings
            .Include(l => l.Product)
            .Include(l => l.Seller)
            .Where(l => l.SellerId == userId)
            .ToListAsync();
    }
}