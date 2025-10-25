namespace StudentBazaar.web.Models
{
    public class Listing
    {
        public int ListingID { get; set; }
        public int ProductID { get; set; }
        public int SellerID { get; set; }
        public decimal Price { get; set; }
        public string Condition { get; set; }
        public decimal? Discount { get; set; }
        public string Status { get; set; }

        // Relationships
        public Product Product { get; set; }
       // public User Seller { get; set; }
    }
}
