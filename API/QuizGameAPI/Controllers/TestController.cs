using Microsoft.AspNetCore.Mvc;
using QuizGameAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizGameAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("dbtest")] // TEST CONNECTION
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                var users = await _context.Users
                    .Take(5)
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database connection failed: {ex.Message}");
            }
        }
    }
}
