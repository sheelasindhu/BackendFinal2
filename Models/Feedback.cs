
namespace SmartFeedbackBackend.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } // ⭐ Add rating
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
