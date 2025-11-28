using System.ComponentModel;


namespace StudentBazaar.Web.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        [DisplayName("Product Name")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please select a category.")]
        public int? CategoryId { get; set; }  // تم تعديل int إلى int? لتفادي مشكلة Required

        [Required]
        [DisplayName("Price")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")]
        public decimal Price { get; set; }

        public int? OwnerId { get; set; }

        public ApplicationUser? Owner { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public ProductCategory? Category { get; set; } 

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
