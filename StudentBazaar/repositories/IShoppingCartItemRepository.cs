namespace StudentBazaar.Web.Repositories
{
    public interface IShoppingCartItemRepository : IGenericRepository<ShoppingCartItem>
    {
        void Update(ShoppingCartItem cartItem);

        // ✅ عدّل الاسم ليتطابق مع الكود
        Task<IEnumerable<ShoppingCartItem>> GetByUserIdAsync(int userId);
    }
}
