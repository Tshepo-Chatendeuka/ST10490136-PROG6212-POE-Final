using Microsoft.AspNetCore.Mvc;
using RaceDay.Contracts;
using RaceDay.Mvc.Services;
namespace RaceDay.Mvc.Controllers;
public class OrganiserController : Controller {
    private readonly RaceDayApiClient _api;
    public OrganiserController(RaceDayApiClient api) => _api = api;
    private bool Auth() => HttpContext.Session.GetString("Role") == "Organiser";

    public async Task<IActionResult> Index() {
        if (!Auth()) return RedirectToAction("Login", "Auth");
        var res = await _api.SendAsync(HttpMethod.Get, "api/events");
        return View(await _api.ReadAsync<List<EventDto>>(res));
    }
    
    [HttpGet] public IActionResult CreateEvent() => Auth() ? View() : RedirectToAction("Login", "Auth");
    [HttpPost]
    public async Task<IActionResult> CreateEvent(CreateEventRequest req, IFormFile banner) {
        if (!Auth()) return RedirectToAction("Login", "Auth");
        var res = await _api.SendAsync(HttpMethod.Post, "api/events", req);
        if (res.IsSuccessStatusCode && banner != null) {
            var ev = await _api.ReadAsync<EventDto>(res);
            await _api.UploadFileAsync($"api/events/{ev!.EventId}/banner", banner);
        }
        return RedirectToAction(nameof(Index));
    }
}
