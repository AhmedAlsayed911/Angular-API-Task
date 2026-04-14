using Madrasty_API.AuthModels;
using Madrasty_API.Entities;
using Madrasty_API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Madrasty_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<ApplicationUser> userManager, Jwt jwt)
        : ControllerBase
    {
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Format");

            var checkEmail = await userManager.FindByEmailAsync(model.Email);
            var checkUserName = await userManager.FindByNameAsync(model.UserName);

            if (checkUserName is not null || checkEmail is not null)
                return BadRequest("Username or Email already exists");

            var newUser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
            };

            var createUser = await userManager.CreateAsync(newUser, model.Password);

            if (!createUser.Succeeded)
                return BadRequest(createUser.Errors.Select(e => e.Description).ToList());

            await userManager.AddToRoleAsync(newUser, "Student");

            return Ok("Account Registred Successfully");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Format");
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return NotFound("Email not found");
            var checkPassword = await userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPassword)
                return BadRequest("Incorrect Password");

            var userRoles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>(){
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwt.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(jwt.DurationInMinutes)
                );


            return Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
        }
    }
}
