namespace StudentBazaar.web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class College
    {
        [Key]
        public int CollegeID { get; set; }

        [Required]
        [MaxLength(150)]
        public string CollegeName { get; set; }

        // ==========================
        // 🔗 Relationships
        // ==========================

        // Relationship with University (Many Colleges -> One University)
        [Required]
        public int UniversityID { get; set; }

        [ForeignKey("UniversityID")]
        public University University { get; set; }

        // Relationship with Users (One College -> Many Users)
        public ICollection<User> Users { get; set; }
    }
}
