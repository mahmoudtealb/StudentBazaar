namespace StudentBazaar.web.Models
{
    public class StudyYear
    {
        public int YearID { get; set; }
        public int MajorID { get; set; }
        public string YearName { get; set; } = string.Empty;

        // Relationships
        public Major Major { get; set; }
    }
}
