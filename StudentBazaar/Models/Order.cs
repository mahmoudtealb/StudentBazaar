

namespace StudentBazaar.Web.Models;

public class Order : BaseEntity
{
    [Required]
    public int ListingId { get; set; } // FK
    public virtual Listing Listing { get; set; } = null!; 

    [Required]
    public int BuyerId { get; set; } // FK
    public virtual User Buyer { get; set; } = null!; 

    // ==========================
    // 📅 General Info
    // ==========================

    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Required]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;

    // ==========================
    // 💰 Financial Details
    // ==========================

    [Range(0, 100)]
    public decimal SiteCommission { get; set; } = 5.0m;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, (double)decimal.MaxValue)]
    public decimal TotalAmount { get; set; }

    // ==========================
    // 🚚 Relationships (1:1)
    // ==========================

    public virtual Shipment? Shipment { get; set; }
}
