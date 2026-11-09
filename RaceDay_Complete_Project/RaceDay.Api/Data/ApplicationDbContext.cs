using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Models;
namespace RaceDay.Api.Data;
public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<SystemRole> SystemRoles { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Event> Events { get; set; }
}
