
namespace StudentBazaar.Web.Models
{
    public class ApplicationUser : IdentityUser<int> // Id نوعه int
    {
        // ==========================
        // 🔹 Basic Info
        // ==========================
        [Required]
        [MaxLength(100)]
        [PersonalData] // للحفاظ على البيانات الشخصية
        public string FullName { get; set; } = string.Empty;

        [MaxLength(250)]
        [PersonalData]
        public string Address { get; set; } = string.Empty;

        // ==========================
        // 🔹 Relationships (FK)
        // ==========================
        [Required]
        public int UniversityId { get; set; }

        [ForeignKey(nameof(UniversityId))]
        public University University { get; set; } = null!;

        [Required]
        public int CollegeId { get; set; }

        [ForeignKey(nameof(CollegeId))]
        public College College { get; set; } = null!;

        // ==========================
        // 🔹 Reverse Relationships
        // ==========================
        public ICollection<Listing> ListingsPosted { get; set; } = new List<Listing>();
        public ICollection<Order> OrdersPlaced { get; set; } = new List<Order>();
        public ICollection<Rating> RatingsGiven { get; set; } = new List<Rating>();
        public ICollection<Shipment> ShipmentsHandled { get; set; } = new List<Shipment>();
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
    }
}
