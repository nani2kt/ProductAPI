using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Replace this with real user validation.
        if (model.Username == "admin" && model.Password == "password")
        {
            // These values can be stored in configuration (e.g., appsettings.json)
            var secretKey = "WECoU7GxZtR1GK0Ho92qscv3WG5iL69/RaC1yR7frFg="; // Base64-encoded key
            var issuer = "dotnet-user-jwts";
            var audience = "https://localhost:44301";

            // Convert the base64 secret key to bytes
            var keyBytes = Convert.FromBase64String(secretKey);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            // Create signing credentials using HMAC-SHA256
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create token claims. You can add additional claims as needed.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1), // Token valid for 1 hour
                signingCredentials: credentials);

            // Serialize the token to a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }
        return Unauthorized();
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}