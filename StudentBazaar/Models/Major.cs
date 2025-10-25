using System.ComponentModel.DataAnnotations;

namespace StudentBazaar.web.Models
{
    public class Major
    {
        public int MajorId { get; set; }
        public int CollegeID { get; set; }

        [MaxLength(length:250)]
        public string MajorName { get; set; } = string.Empty;

        public College College { get; set; }
        public ICollection<StudyYear> StudyYears { get; set; }
    }
}
