namespace StudentBazaar.web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductCategory
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(150)]
        public string CategoryName { get; set; }

        // ==========================
        // 🔗 Relationships
        // ==========================

        // One Category -> Many Products
        public ICollection<Product> Products { get; set; }
    }
}
