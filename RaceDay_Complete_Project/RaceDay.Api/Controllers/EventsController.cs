using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using RaceDay.Api.Services;
using RaceDay.Contracts;
namespace RaceDay.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase {
    private readonly ApplicationDbContext _db;
    public EventsController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _db.Events.Select(e => new EventDto { EventId = e.EventID, Name = e.Name, Description = e.Description, EventDate = e.EventDate, Location = e.Location, DistanceKm = e.Distance, EventType = e.EventType, BannerImageUrl = e.BannerImageUrl }).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Post(CreateEventRequest req) {
        if (HttpContext.Session.GetString("Role") != "Organiser") return Unauthorized();
        var ev = new Event { Name = req.Name, Description = req.Description, EventDate = req.EventDate, Location = req.Location, Distance = req.DistanceKm, EventType = req.EventType, OrganiserID = HttpContext.Session.GetInt32("UserID") ?? 0 };
        _db.Events.Add(ev); await _db.SaveChangesAsync();
        return Created("", new EventDto { EventId = ev.EventID });
    }

    [HttpPost("{id}/banner")]
    public async Task<IActionResult> UploadBanner(int id, IFormFile file, [FromServices] BlobStorageService storage) {
        if (HttpContext.Session.GetString("Role") != "Organiser") return Unauthorized();
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();
        ev.BannerImageUrl = await storage.UploadAsync(file, "banners");
        await _db.SaveChangesAsync();
        return Ok(new { imageUrl = ev.BannerImageUrl });
    }
}
