using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace AuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;


        public AuthenticationController(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;

        }


        [HttpPost("register")]
        public async Task<IActionResult> Resgister([FromBody] UserResigterDTO user)
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

          public async Task<IActionResult> Login([FromBody] UserDTO user)
          {
                var userDB = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username != null && u.Username == user.Username);
                if(userDB == null || !BCrypt.Net.BCrypt.Verify(user.Password, userDB.Passwordhash))
                {
                    return BadRequest("Invalid username or password");
                }
                var token = GenerateJWTToken(userDB);
                return Ok(token);
          }

        private string GenerateJWTToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"] ?? ""));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AuthorizedEndpoint()
        {
                return Ok("You are authorized");
        }



    }

 
 }