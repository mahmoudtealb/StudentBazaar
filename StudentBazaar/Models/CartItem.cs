public class CartItem
{
    public int Id { get; set; }

    public string UserId { get; set; } // لو UserManager يستخدم string Id
    public ApplicationUser User { get; set; }
    public int ListingId { get; set; }
    public Listing Listing { get; set; }

    // الكمية
    public int Quantity { get; set; }

    // تواريخ اختيارية
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}
