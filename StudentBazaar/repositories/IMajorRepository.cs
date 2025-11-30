namespace StudentBazaar.Web.Repositories;

public interface IMajorRepository : IGenericRepository<Major>
{
    void Update(Major major);
}