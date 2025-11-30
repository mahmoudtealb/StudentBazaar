using System.ComponentModel.DataAnnotations;

namespace StudentBazaar.Web.Models
{
    public class ProductCategory : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
