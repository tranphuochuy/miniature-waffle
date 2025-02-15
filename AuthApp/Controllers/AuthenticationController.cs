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
            if((user.Username == null || user.Password == null) || (string.IsNullOrEmpty(user.Username) && string.IsNullOrEmpty(user.Password)))
            {
                return BadRequest("Username or password is missing");
            }
            var userDB = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username != null && u.Username == user.Username);
            var userpassDB = await _dbContext.Users.FirstOrDefaultAsync(u => u.Passwordhash != null && u.Passwordhash == user.Password);

            if(userDB != null)
            {
                return BadRequest("Username already exists");
            }
            else
            {
                return BadRequest("Username does not exist");
            }

            if(userpassDB != null)
            {
                return BadRequest("Password already exists");
            }

            var newUser = new User
            {
                Username = user.Username,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return Ok("User created successfully");
        }
    }
}