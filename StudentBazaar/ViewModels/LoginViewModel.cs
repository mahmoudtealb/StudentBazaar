// ملف: Models/ViewModels/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace StudentBazaar.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Student"; // Default role
    }
}
