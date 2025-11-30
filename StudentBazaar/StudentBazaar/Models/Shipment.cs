

namespace StudentBazaar.Web.Models
{
    public class Shipment : BaseEntity
    {
        // Foreign Key to Order (1:1 relationship)
        [Required]
        public int OrderId { get; set; } // FK updated

        // Foreign Key to User (Shipper Role)
        [Required]
        public int ShipperId { get; set; } // FK updated

        [Required]
        [MaxLength(100)]
        public string TrackingNumber { get; set; } = string.Empty;

        [Required]
        public ShipmentStatus Status { get; set; } = ShipmentStatus.AwaitingPickup; 

        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.Now.AddDays(7);

        // ==========================
        // 🔗 Relationships
        // ==========================

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!; 

        [ForeignKey("ShipperId")]
        public virtual ApplicationUser Shipper { get; set; } = null!; 
    }
}
