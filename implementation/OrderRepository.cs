

namespace StudentBazaar.Web.implementation;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private readonly ApplicationDbContext _context;
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
    }

    public async Task<IEnumerable<Order>> GetByUserAsync(int userId)
    {
        return await _context.Orders
            .Include(o => o.Buyer)
            .Include(o => o.Listing)
                .ThenInclude(l => l.Product)
            .Where(o => o.BuyerId == userId)
            .ToListAsync();
    }

}

