using Microsoft.EntityFrameworkCore;

namespace StudentBazaar.web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options)
        : base(options)
    {
        
    }
    // This section defines the Fluent API configuration 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
