namespace StudentBazaar.web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        // ==========================
        // 🔗 Relationships
        // ==========================

        // Relationship with University (Many Users -> One University)
        [Required]
        public int UnivID { get; set; }

        [ForeignKey("UnivID")]
        public required University University { get; set; }

        // Relationship with College (Many Users -> One College)
        [Required]
        public int CollegeID { get; set; }

        [ForeignKey("CollegeID")]
        public College College { get; set; }
    }
}
