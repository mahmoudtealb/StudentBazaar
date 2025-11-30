
namespace StudentBazaar.Web.Models;

public abstract class BaseEntity
{
    // Primary Key (PK) - EF Core convention prefers 'Id'
    [Key]
    public int Id { get; set; }

    // Record creation timestamp
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Timestamp for last update
    public DateTime? UpdatedAt { get; set; }
}
