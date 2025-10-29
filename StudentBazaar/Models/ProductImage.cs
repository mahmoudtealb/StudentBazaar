namespace StudentBazaar.Web.Models;

public class ProductImage:BaseEntity
{
    // Path/URL to the image file
    [Required]
    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    // Is this the main image to be displayed in the listing?
    public bool IsMainImage { get; set; } = false;

    // ==========================
    // 🔗 Relationships (Many Images -> One Product)
    // ==========================

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}
