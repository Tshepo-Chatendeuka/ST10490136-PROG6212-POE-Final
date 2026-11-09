using Microsoft.AspNetCore.Mvc;
using RaceDay.Contracts;
using RaceDay.Mvc.Services;
using RaceDay.Mvc.ViewModels;
namespace RaceDay.Mvc.Controllers;
public class AuthController : Controller {
    private readonly RaceDayApiClient _api;
    public AuthController(RaceDayApiClient api) => _api = api;

    [HttpGet] public IActionResult Login() => View();
    [HttpGet] public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm) {
        if (!ModelState.IsValid) return View(vm);
        var res = await _api.SendAsync(HttpMethod.Post, "api/auth/login", new LoginRequest { Email = vm.Email, Password = vm.Password });
        if (!res.IsSuccessStatusCode) { ModelState.AddModelError("", "Invalid login"); return View(vm); }
        
        var auth = await _api.ReadAsync<AuthResponse>(res);
        string? cookie = RaceDayApiClient.ExtractSessionCookie(res);
        if (cookie != null) HttpContext.Session.SetString("ApiSessionCookie", cookie);
        
        HttpContext.Session.SetInt32("UserId", auth!.UserId);
        HttpContext.Session.SetString("Role", auth.Role);
        return RedirectToAction("Index", auth.Role == "Organiser" ? "Organiser" : "Participant");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm vm) {
        var res = await _api.SendAsync(HttpMethod.Post, "api/auth/register", new RegisterRequest { FirstName = vm.FirstName, LastName = vm.LastName, Email = vm.Email, Password = vm.Password, Role = vm.Role });
        if (!res.IsSuccessStatusCode) { ModelState.AddModelError("", "Error registering"); return View(vm); }
        return RedirectToAction(nameof(Login));
    }
    
    [HttpPost] public async Task<IActionResult> Logout() { await _api.SendAsync(HttpMethod.Post, "api/auth/logout"); HttpContext.Session.Clear(); return RedirectToAction("Index", "Home"); }
}
