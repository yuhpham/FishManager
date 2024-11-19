using KoiFishManager.Api.Entities;
using KoiFishManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KoiFishManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginController(IConfiguration configuration,
                               UserManager<User> userManager,
                               SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            // find in DB
            User? user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                return BadRequest(new LoginResponse { Successful = false, Error = "Username and password are invalid." });
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest(new LoginResponse { Successful = false, Error = "Username and password are invalid." });
            }

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, login.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
            DateTime expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            JwtSecurityToken token = new(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new LoginResponse { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {      
            if (register == null)
            {
                return BadRequest("Invalid registration data.");
            }
     
            User? existingUser = await _userManager.FindByNameAsync(register.UserName);
            if (existingUser != null)
            {
                return BadRequest(new RegisterResponse { Successful = false, Error = "Username already exists." });
            }

            User? existingEmail = await _userManager.FindByEmailAsync(register.Email);
            if (existingEmail != null)
            {
                return BadRequest(new RegisterResponse { Successful = false, Error = "Email already exists." });
            }

            User user = new()
            {
                UserName = register.UserName,
                Email = register.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new RegisterResponse { Successful = false, Error = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            return Ok(new RegisterResponse { Successful = true, Message = "Registration successful!" });
        }

    }
}
