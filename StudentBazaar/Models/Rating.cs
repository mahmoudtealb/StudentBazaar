
namespace StudentBazaar.Web.Models;
public class Rating : BaseEntity
{
    // User who created the rating
    [Required]
    public int UserId { get; set; } // FK updated
    public virtual ApplicationUser User { get; set; } = null!; 

    // Product being rated
    [Required]
    public int ProductId { get; set; } // FK updated
    public virtual Product Product { get; set; } = null!; 

    // Rating stars (1–5)
    [Required]
    [Range(1, 5)]
    public int Stars { get; set; }

    // Optional comment about the product
    [MaxLength(500)]
    public string? Comment { get; set; } // ✅ Nullable allowed (optional field)
}
