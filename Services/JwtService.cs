using Microsoft.IdentityModel.Tokens;
using onlineBookstoreAPI.Data;
using onlineBookstoreAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService(IConfiguration config)
{
    private readonly string _secret = config["Jwt:Key"]!;
    private readonly string _issuer = config["Jwt:Issuer"]!;
    private readonly string _configuration = config["Jwt:Audience"]!;

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secret)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _configuration, 
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? ""),
                new Claim(ClaimTypes.Name, user.Name ?? "")
            },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
