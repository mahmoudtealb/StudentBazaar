namespace StudentBazaar.web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class University
    {
        [Key]
        public int UniversityID { get; set; }

        [Required]
        [MaxLength(150)]
        public string UniversityName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; }

        // ==========================
        // 🔗 Relationships
        // ==========================

        // One University -> Many Colleges
        public ICollection<College> Colleges { get; set; }

        // One University -> Many Users
        public ICollection<User> Users { get; set; }
    }
}
