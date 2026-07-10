using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizGameAPI.Models;

namespace QuizGameAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("register")] // REGISTER
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("Username and password are required.");

            var exists = await _context.Users.AnyAsync(u => u.username == userDto.Username);
            if (exists) return BadRequest("Username already taken.");

            var user = new User
            {
                username = userDto.Username,
                password_hash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User created.");
        }

        [HttpPost("login")] // LOGIN
        public async Task<IActionResult> Login(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("Username and password are required.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == userDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.password_hash))
                return Unauthorized("Invalid credentials.");

            return Ok(new { user.id, user.username });
        }


        [HttpGet("test")] // TEST
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(new { success = true, message = $"Connected! Found {users.Count} users." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Failed to connect to the database.", error = ex.Message });
            }
        }

        [HttpPost("update-category-highscore")] // UPDATE HIGHSCORE FOR CATEGORY
        public async Task<IActionResult> UpdateCategoryHighscore(UpdateCategoryHighscoreDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == dto.Username);
            if (user == null) return NotFound("User not found.");

            var highscore = await _context.Highscores
                .FirstOrDefaultAsync(h => h.UserId == user.id && h.Category == dto.Category);

            if (highscore == null)
            {
                _context.Highscores.Add(new Highscores
                {
                    UserId = user.id,
                    Category = dto.Category,
                    Highscore = dto.Score
                });
            }
            else if (dto.Score > highscore.Highscore)
            {
                highscore.Highscore = dto.Score;
                _context.Highscores.Update(highscore);
            }

            await _context.SaveChangesAsync();
            return Ok("Highscore recorded.");
        }

        [HttpGet("leaderboard/{category}")] // LEADERBOARD
        public async Task<IActionResult> GetLeaderboard(string category)
        {
            var topScores = await _context.Highscores
                .Where(h => h.Category == category)
                .OrderByDescending(h => h.Highscore)
                .Take(10)
                .Select(h => new
                {
                    h.User.username,
                    h.Highscore
                })
                .ToListAsync();

            return Ok(topScores);
        }


        public class UserDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class UpdateCategoryHighscoreDto
        {
            public string Username { get; set; }
            public string Category { get; set; }
            public int Score { get; set; }
        }
    }
}