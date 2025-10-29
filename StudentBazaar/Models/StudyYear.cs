
namespace StudentBazaar.Web.Models
{
    public class StudyYear : BaseEntity
    {

        [Required]
        [MaxLength(100)]
        public string YearName { get; set; } = string.Empty; 

        // ==========================
        // 🔗 Relationships (Many StudyYears -> One Major)
        // ==========================

        [Required]
        public int MajorId { get; set; }

        [ForeignKey(nameof(MajorId))]
        public Major Major { get; set; } = null!; // 

        // ==========================
        // 🔁 Reverse Relationships (One StudyYear -> Many Products)
        // ==========================

        public ICollection<Product> Products { get; set; } = new List<Product>(); 
    }
}
