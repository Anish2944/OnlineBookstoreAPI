using Microsoft.AspNetCore.Mvc;
using onlineBookstoreAPI.Models;
using onlineBookstoreAPI.Data;
using onlineBookstoreAPI.Common;
using onlineBookstoreAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;
    public AuthController(AppDbContext db, JwtService jwt) { _db = db; _jwt = jwt; }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDtos.UserRegisterDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, null, "Invalid input"));

        var normalized = request.Email.Trim().ToLowerInvariant();
        var exists = await _db.Users.AnyAsync(u => u.Email.ToLower() == normalized);
        if (exists) return Conflict(new ApiResponse<object>(false, null, "Email already exists"));

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = normalized,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role,
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _jwt.GenerateToken(user); // make sure it includes "sub", "email", "role"
        return Ok(new ApiResponse<AuthDtos.AuthResponseDto>(true, new AuthDtos.AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        }));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDtos.UserLoginDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>(false, null, "Invalid input"));

        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new ApiResponse<object>(false, null, "Invalid credentials"));

        var token = _jwt.GenerateToken(user);
        return Ok(new ApiResponse<AuthDtos.AuthResponseDto>(true, new AuthDtos.AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        }));
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = User.GetUserId();
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Unauthorized(new { message = "Token not authenticated" });
        }

        if (id is null) return Unauthorized(new ApiResponse<object>(false, null, "Not logged in"));
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return Unauthorized(new ApiResponse<object>(false, null, "User not found"));

        return Ok(new ApiResponse<object>(true, new { user.Id, user.Name, user.Email, user.Role }));
    }
    //[HttpGet("debug-token")]
    //public IActionResult DebugToken()
    //{
    //    var auth = Request.Headers["Authorization"].FirstOrDefault();
    //    if (string.IsNullOrEmpty(auth)) return Ok(new { authHeader = auth, message = "no header" });

    //    if (!auth.StartsWith("Bearer ")) return Ok(new { authHeader = auth, message = "header not start with 'Bearer '" });

    //    var raw = auth.Substring("Bearer ".Length).Trim();
    //    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    //    var jwt = handler.ReadJwtToken(raw);
    //    return Ok(new
    //    {
    //        header = jwt.Header,
    //        payload = jwt.Payload,
    //        authHeader = auth,
    //        tokenLength = raw.Length
    //    });
    //}

}
