

namespace StudentBazaar.Web.Models
{
    public class Listing : BaseEntity
    {
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public ListingCondition Condition { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Discount { get; set; }

        // ✅ كان string بقى Enum
        [Required]
        public ListingStatus Status { get; set; } = ListingStatus.Available;

        [Required]
        public DateTime PostingDate { get; set; } = DateTime.Now;

        // ==========================
        // 🔗 Foreign Keys
        // ==========================
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SellerId { get; set; }

        // ==========================
        // 🔗 Navigation Properties
        // ==========================
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(SellerId))]
        public User Seller { get; set; } = null!;

        // ==========================
        // 🔁 Reverse Relationships
        // ==========================
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>(); // ✅ initialized
    }
}
