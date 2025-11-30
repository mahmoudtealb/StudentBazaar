
namespace StudentBazaar.Web.Models
{
    public class Major : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string MajorName { get; set; } = string.Empty; 
        // ==========================
        // 🔗 Relationships (Many Majors -> One College)
        // ==========================

        [Required]
        public int CollegeId { get; set; }

        [ForeignKey(nameof(CollegeId))]
        public College College { get; set; } = null!; // 

        // ==========================
        // 🔁 Reverse Relationships (One Major -> Many StudyYears)
        // ==========================

    //    public ICollection<StudyYear> StudyYears { get; set; } = new List<StudyYear>(); 

    }
}
