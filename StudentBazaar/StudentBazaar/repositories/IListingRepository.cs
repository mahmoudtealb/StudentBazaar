namespace StudentBazaar.Web.Repositories;

public interface IListingRepository : IGenericRepository<Listing>
{
    void Update(Listing listing);
    Task<IEnumerable<Listing>> GetByUserAsync(int userId);
}
