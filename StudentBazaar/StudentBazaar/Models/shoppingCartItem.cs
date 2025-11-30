namespace StudentBazaar.Web.Models;

public class ShoppingCartItem : BaseEntity
{
    // ==========================
    // 🔗 Foreign Keys
    // ==========================

    [Required]
    public int UserId { get; set; } // The user who owns this cart item

    [Required]
    public int ListingId { get; set; } // The product listing added to the cart

    // ==========================
    // 🔗 Navigation Properties
    // ==========================

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    [ForeignKey(nameof(ListingId))]
    public Listing Listing { get; set; } = null!;

    // Quantity (usually 1 for used items, but added for flexibility)
    [Range(1, 10)]
    public int Quantity { get; set; } = 1;
}