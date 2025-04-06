using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserServiceMS.ContextDB;
using UserServiceMS.Helpers;
using UserServiceMS.Models;

namespace UserServiceMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IHelper _jwtHelper;

        public UserController(UserDbContext context, IHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Username already exists.");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || user.PasswordHash != HashPassword(request.Password))
                return Unauthorized("Invalid credentials.");

            var token = _jwtHelper.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return NotFound();

            return Ok(new { user.Id, user.Username, user.Role });
        }

        [Authorize(Roles = "Manager,HR/Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new { u.Id, u.Username, u.Role })
                .ToListAsync();
            return Ok(users);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
