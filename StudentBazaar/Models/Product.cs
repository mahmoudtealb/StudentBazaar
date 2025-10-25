using System.Reflection;

namespace StudentBazaar.web.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CatID { get; set; }
        public string Name { get; set; } = string.Empty;

        // Relationships
        public ProductCategory Category { get; set; }
        public ICollection<Listing> Listings { get; set; }
    }
}
