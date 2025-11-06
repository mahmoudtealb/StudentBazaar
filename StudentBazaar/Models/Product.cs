using System.ComponentModel;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace StudentBazaar.Web.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        [DisplayName("Product Name")] // Added DisplayName for UI clarity
        public string Name { get; set; } = string.Empty;

        // ==========================
        // 🔗 Foreign Keys
        // ==========================

        [Required]
        [DisplayName("Category")] // Added DisplayName
        public int CategoryId { get; set; }

        [Required]
        [DisplayName("Study Year")] // Added DisplayName
        public int StudyYearId { get; set; }

        // ==========================
        // 🔗 Navigation Properties (Many Products -> One Category/StudyYear)
        // ==========================

        [ForeignKey(nameof(CategoryId))]
        public ProductCategory Category { get; set; } = null!;

        [ForeignKey(nameof(StudyYearId))]
        public StudyYear StudyYear { get; set; } = null!;

        // ==========================
        // 🔁 Reverse Relationships (One Product -> Many Listings/Ratings/Images)
        // ==========================

        // MODIFIED: Replaced the single 'Img' string with a collection of images
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        // One Product can have multiple listings (e.g., different conditions/sellers)
        public ICollection<Listing> Listings { get; set; } = new List<Listing>();

        // One Product can have multiple ratings
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
