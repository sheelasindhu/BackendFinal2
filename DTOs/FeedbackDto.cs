namespace SmartFeedbackBackend.DTOs;

public class FeedbackDto
{
    public string Category { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } // ⭐ Include rating (1 to 5)
}
