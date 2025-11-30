namespace StudentBazaar.Web.Repositories;

public interface IUniversityRepository: IGenericRepository<University>
{
    void Update(University university);
}
