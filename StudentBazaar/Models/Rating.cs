using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBazaar.web.Models;
public class Rating
{
   
    [Key]
    public int RatingID { get; set; }

    // User who created the rating
    [Required]
    [ForeignKey("User")]
    public int UserID { get; set; }
    public virtual User User { get; set; }

    // Product being rated
    [Required]
    [ForeignKey("Product")]
    public int ProductID { get; set; }
    public virtual Product Product { get; set; }

    // Rating stars (1–5)
    [Range(1, 5)]
    public int Stars { get; set; }

    // Optional comment about the product
    [MaxLength(500)]
    public string? Comment { get; set; }

    // Rating creation date
    public DateTime Date { get; set; } = DateTime.Now;
}
