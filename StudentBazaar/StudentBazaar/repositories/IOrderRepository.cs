namespace StudentBazaar.Web.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    void Update(Order order);
    Task<IEnumerable<Order>> GetByUserAsync(int userId);
}