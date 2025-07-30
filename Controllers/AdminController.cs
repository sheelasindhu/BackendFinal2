using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartFeedbackBackend.Data;
using SmartFeedbackBackend.Models;

namespace SmartFeedbackAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Select(f => new
                {
                    f.Id,
                    f.Category,
                    f.Content,
                    f.Rating,
                    f.CreatedAt,
                    UserName = f.User != null ? f.User.Name : "Unknown"
                })
                .ToListAsync();

            return Ok(feedbacks);
        }

        // GET: api/admin/feedbacks/count
        [HttpGet("feedbacks/count")]
        public async Task<IActionResult> GetFeedbackCount()
        {
            int count = await _context.Feedbacks.CountAsync();
            return Ok(count);
        }

        // GET: api/admin/users/count
        [HttpGet("users/count")]
        public async Task<IActionResult> GetUserCount()
        {
            int count = await _context.Users.CountAsync();
            return Ok(count);
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetFeedbackAnalytics()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();

            if (!feedbacks.Any())
            {
                return Ok(new
                {
                    CategoryCounts = new Dictionary<string, int>(),
                    CategoryRatings = new Dictionary<string, double>(),
                    ContentRatings = new Dictionary<string, double>(),
                    ContentCounts = new Dictionary<string, int>(),
                    AverageRating = 0,
                    TotalFeedbacks = 0
                });
            }

            var categoryCounts = feedbacks
                .GroupBy(f => f.Category)
                .ToDictionary(g => g.Key, g => g.Count());

            var categoryRatings = feedbacks
                .GroupBy(f => f.Category)
                .ToDictionary(g => g.Key, g => Math.Round(g.Average(f => f.Rating), 2));

            var contentCounts = feedbacks
                .GroupBy(f => f.Content)
                .ToDictionary(g => g.Key, g => g.Count());

            var contentRatings = feedbacks
                .GroupBy(f => f.Content)
                .ToDictionary(g => g.Key, g => Math.Round(g.Average(f => f.Rating), 2));

            var averageRating = Math.Round(feedbacks.Average(f => f.Rating), 2);
            var totalFeedbacks = feedbacks.Count;

            return Ok(new
            {
                CategoryCounts = categoryCounts,
                CategoryRatings = categoryRatings,
                ContentCounts = contentCounts,
                ContentRatings = contentRatings,
                AverageRating = averageRating,
                TotalFeedbacks = totalFeedbacks
            });
        }


    }
}
