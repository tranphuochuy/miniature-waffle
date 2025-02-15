using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AuthenticationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Resgister(UserDTO user)
        {
            if(await _dbContext.Users.FirstOrDefaultAsync(u => u.Username != null && u.Username == user.Username) != null)
            {
                return BadRequest("Username already exists");
            }
            if(await _dbContext.Users.FirstOrDefaultAsync(u => u.Passwordhash != null && u.Passwordhash == user.Passwordhash) != null)
            {
                return BadRequest("Password already exists");
            }

            var newUser = new User
            {
                Username = user.Username,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(user.Passwordhash)
            };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return Ok("User created successfully");
        }
    }
}