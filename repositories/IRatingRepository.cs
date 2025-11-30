namespace StudentBazaar.Web.Repositories;

public interface IRatingRepository : IGenericRepository<Rating>
{
    void Update(Rating rating);

}
