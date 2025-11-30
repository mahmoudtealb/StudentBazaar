namespace StudentBazaar.Web.Repositories;

public interface ICollegeRepository :IGenericRepository<College>
{
  void Update (College college);    
}