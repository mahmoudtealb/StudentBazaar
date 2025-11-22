

namespace StudentBazaar.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Register As")]
        public string Role { get; set; } = "Student"; // Default to Student

        [Required]
        public int UniversityId { get; set; }

        [Required]
        public int CollegeId { get; set; }

        public string Address { get; set; } = string.Empty;

        // Dropdowns
        public IEnumerable<SelectListItem> Universities { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Colleges { get; set; } = new List<SelectListItem>();
    }
}
