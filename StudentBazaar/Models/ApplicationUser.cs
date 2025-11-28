using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace StudentBazaar.Web.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        // ==========================
        // 🔹 Basic Info
        // ==========================
        [Required]
        [MaxLength(100)]
        [PersonalData]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(250)]
        [PersonalData]
        public string Address { get; set; } = string.Empty;

        // ==========================
        // 🔹 University & College (Optional for Admin)
        // ==========================

        public int? UniversityId { get; set; }   // ❗ Nullable now

        [ForeignKey(nameof(UniversityId))]
        public University? University { get; set; }  // ❗ Nullable navigation

        public int? CollegeId { get; set; }      // ❗ Nullable now

        [ForeignKey(nameof(CollegeId))]
        public College? College { get; set; }    // ❗ Nullable navigation

        // ==========================
        // 🔹 Reverse Relationships
        // ==========================
        public ICollection<Listing> ListingsPosted { get; set; } = new List<Listing>();
        public ICollection<Order> OrdersPlaced { get; set; } = new List<Order>();
        public ICollection<Rating> RatingsGiven { get; set; } = new List<Rating>();
        public ICollection<Shipment> ShipmentsHandled { get; set; } = new List<Shipment>();
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

        public ICollection<ChatMessage> MessagesSent { get; set; } = new List<ChatMessage>();
        public ICollection<ChatMessage> MessagesReceived { get; set; } = new List<ChatMessage>();

    }
}
