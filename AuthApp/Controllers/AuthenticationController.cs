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
        public async Task<IActionResult> Resgister(UserResigterDTO user)
        {
            if((user.Username == null || user.Password == null) || (string.IsNullOrEmpty(user.Username) && string.IsNullOrEmpty(user.Password)))
            {
                return BadRequest("Username or password is missing");
            }
            var userDB = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if(userDB != null && !string.IsNullOrEmpty(userDB.Username) && user.Username == userDB.Username)
            {
                
                return BadRequest("Username already exists");
            }

            if(user.Password != user.ConfirmPassword)
            {
                return BadRequest("Password do not Match");
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

          [HttpGet("login")]

          public async Task<IActionResult> Login(UserDTO user)
          {
                var userDB = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username != null && u.Username == user.Username);
                if(userDB == null || !BCrypt.Net.BCrypt.Verify(user.Password, userDB.Passwordhash))
                {
                    return BadRequest("Invalid username or password");
                }

                return Ok("Login successful");
          }


    }
}