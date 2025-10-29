namespace StudentBazaar.Web.Models
{
  

    public class College : BaseEntity
    {

        [Required]
        [MaxLength(150)]
        public string CollegeName { get; set; } = string.Empty; 

        // ==========================
        // 🔗 Relationships (Many Colleges -> One University)
        // ==========================

        [Required]
        public int UniversityId { get; set; }

        [ForeignKey(nameof(UniversityId))]
        public University University { get; set; } = null!; 

        // ==========================
        // 🔗 Reverse Relationships (One College -> Many Users/Majors)
        // ==========================

        public ICollection<User> Users { get; set; } = new List<User>(); 
        public ICollection<Major> Majors { get; set; } = new List<Major>(); 
    }
}
