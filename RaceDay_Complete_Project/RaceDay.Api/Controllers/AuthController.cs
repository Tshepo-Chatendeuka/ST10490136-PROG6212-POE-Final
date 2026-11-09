using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using RaceDay.Contracts;
namespace RaceDay.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly ApplicationDbContext _db;
    public AuthController(ApplicationDbContext db) => _db = db;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req) {
        var role = await _db.SystemRoles.FirstOrDefaultAsync(r => r.RoleName == req.Role) ?? new SystemRole { RoleName = req.Role };
        var user = new AppUser { FirstName = req.FirstName, LastName = req.LastName, Email = req.Email, Role = role, PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password) };
        _db.AppUsers.Add(user);
        await _db.SaveChangesAsync();
        return Created("", new { Message = "Registered" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req) {
        var user = await _db.AppUsers.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash)) return Unauthorized();
        HttpContext.Session.SetInt32("UserID", user.UserID);
        HttpContext.Session.SetString("Role", user.Role!.RoleName);
        return Ok(new AuthResponse { UserId = user.UserID, Role = user.Role.RoleName, FirstName = user.FirstName, Email = user.Email });
    }
    
    [HttpPost("logout")]
    public IActionResult Logout() { HttpContext.Session.Clear(); return Ok(); }
}
