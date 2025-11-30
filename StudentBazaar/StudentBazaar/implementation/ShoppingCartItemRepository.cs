
namespace StudentBazaar.Web.Implementation
{
    public class ShoppingCartItemRepository : GenericRepository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ShoppingCartItem cartItem)
        {
            _context.ShoppingCartItems.Update(cartItem);
        }

        // ✅ Get all cart items for a specific user
        public async Task<IEnumerable<ShoppingCartItem>> GetByUserIdAsync(int userId)
        {
            return await _context.ShoppingCartItems
                .Include(i => i.Listing)   // ✅ بدل Product
                .Where(i => i.UserId == userId) // ✅ بدل ShoppingCartId
                .ToListAsync();
        }
    }
}
