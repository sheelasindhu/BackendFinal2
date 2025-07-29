using Microsoft.EntityFrameworkCore;
using SmartFeedbackBackend.Models;

namespace SmartFeedbackBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
}
