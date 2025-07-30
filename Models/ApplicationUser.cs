using System.ComponentModel.DataAnnotations;

namespace SmartFeedbackBackend.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public string Role { get; set; } = "User";  // Either "User" or "Admin"

        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}