using Microsoft.AspNetCore.Mvc;
using RaceDay.Contracts;
using RaceDay.Mvc.Services;
namespace RaceDay.Mvc.Controllers;
public class ParticipantController : Controller {
    private readonly RaceDayApiClient _api;
    public ParticipantController(RaceDayApiClient api) => _api = api;
    private bool Auth() => HttpContext.Session.GetString("Role") == "Participant";

    public async Task<IActionResult> Index() {
        if (!Auth()) return RedirectToAction("Login", "Auth");
        var res = await _api.SendAsync(HttpMethod.Get, "api/events");
        return View(await _api.ReadAsync<List<EventDto>>(res));
    }
}
