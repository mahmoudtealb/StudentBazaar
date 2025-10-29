namespace StudentBazaar.Web.Models
{

    public class User:BaseEntity
    {

        // Inherits Id, CreatedAt, UpdatedAt

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty; // ADDED: Default value for non-nullable string

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty; // ADDED: Default value for non-nullable string

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public string PasswordHash { get; set; } = string.Empty; // ADDED: Default value, MUST be Hashed

        [Required]
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty; // ADDED: Default value

        [Required]
        public UserRole Role { get; set; } = UserRole.Student;

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        // ==========================
        // 🔗 Relationships (Many Users -> One University/College)
        // ==========================

        [Required]
        public int UniversityId { get; set; } // FK

        [ForeignKey("UniversityId")]
        public required University University { get; set; }

        [Required]
        public int CollegeId { get; set; } // FK

        [ForeignKey("CollegeId")]
        public College College { get; set; } = null!; // ADDED: Null forgiveness operator for non-nullable reference

        // ==========================
        // 🔗 Reverse Relationships (One User -> Many Listings/Orders/Ratings/Cart Items)
        // ==========================

        // Listings posted by this user (Seller)
        public ICollection<Listing> ListingsPosted { get; set; } = new List<Listing>();

        // Orders placed by this user (Buyer)
        public ICollection<Order> OrdersPlaced { get; set; } = new List<Order>();

        // Ratings given by this user
        public ICollection<Rating> RatingsGiven { get; set; } = new List<Rating>();

        // Shipments handled by this user (Shipper)
        public ICollection<Shipment> ShipmentsHandled { get; set; } = new List<Shipment>();

        // ADDED: Shopping Cart Items belonging to this user
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
    }
}
