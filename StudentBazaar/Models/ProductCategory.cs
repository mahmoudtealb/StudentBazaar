    namespace StudentBazaar.Web.Models
    {
  

        public class ProductCategory : BaseEntity
        {
            [Required]
            [MaxLength(150)]
            public string CategoryName { get; set; } = string.Empty; 

            // ==========================
            // 🔗 Relationships (One Category -> Many Products)
            // ==========================

            public ICollection<Product> Products { get; set; } = new List<Product>(); 
        }
    }
