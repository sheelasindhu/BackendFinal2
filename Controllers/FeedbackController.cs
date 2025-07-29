using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFeedbackBackend.Data;
using SmartFeedbackBackend.DTOs;
using SmartFeedbackBackend.Models;
using System.Security.Claims;

namespace SmartFeedbackBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly AppDbContext _context;

    public FeedbackController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize]
    public IActionResult SubmitFeedback(FeedbackDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var feedback = new Feedback
        {
            Category = dto.Category,
            Content = dto.Content,
            UserId = userId
        };

        _context.Feedbacks.Add(feedback);
        _context.SaveChanges();

        return Ok(new
        {
            success = true,
            message = "Feedback submitted successfully and stored in the system.",
            feedback = new
            {
                feedback.Id,
                feedback.Category,
                feedback.Content,
                feedback.CreatedAt
            }
        });
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllFeedback()
    {
        var feedbacks = _context.Feedbacks
            .Select(f => new
            {
                f.Id,
                f.Category,
                f.Content,
                f.CreatedAt,
                User = f.User != null ? f.User.Name : "Unknown"
            }).ToList();
        return Ok(feedbacks);
    }

     [HttpGet("my")]
    [Authorize]
    public IActionResult GetUserFeedback()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var feedbacks = _context.Feedbacks
            .Where(f => f.UserId == userId)
            .Select(f => new
            {
                f.Id,
                f.Category,
                f.Content,
                f.CreatedAt
            }).ToList();

        return Ok(feedbacks);
    }
}
