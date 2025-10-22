using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace StudentBazaar.web.Models;

public class Order
{
    [Key]
    public int OrderID { get; set; }

    // Related listing (the product being purchased)
    [Required]
    [ForeignKey("Listing")]
    public int ListingID { get; set; }
    public virtual Listing Listing { get; set; }

    // The buyer who placed the order
    [Required]
    [ForeignKey("Buyer")]
    public int BuyerID { get; set; }
    public virtual User Buyer { get; set; }

    // Order date
    public DateTime OrderDate { get; set; } = DateTime.Now;

    // Order status (Pending, Confirmed, Shipped, Delivered, Cancelled)
    [Required]
    [MaxLength(50)]
    public string OrderStatus { get; set; } = "Pending";

    // Payment method (Online / CashOnDelivery)
    [Required]
    [MaxLength(30)]
    public string PaymentMethod { get; set; } = "CashOnDelivery";

    // Site commission percentage
    [Range(0, 100)]
    public decimal SiteCommission { get; set; }

    // Total order amount
    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    // Linked shipment (1:1 relationship)
    public virtual Shipment Shipment { get; set; }
}
