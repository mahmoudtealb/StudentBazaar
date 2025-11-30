namespace StudentBazaar.Web.Models
{
    

    public class University:BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string UniversityName { get; set; } = string.Empty; 

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty; 

        // ==========================
        // 🔗 Relationships (One University -> Many Colleges/Users)
        // ==========================

        public ICollection<College> Colleges { get; set; } = new List<College>(); 
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>(); 
    }
}
