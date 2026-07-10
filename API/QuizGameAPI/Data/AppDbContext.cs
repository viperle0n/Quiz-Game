using Microsoft.EntityFrameworkCore;
using QuizGameAPI.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Highscores> Highscores { get; set; }
}
