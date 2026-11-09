using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RaceDay.Api.Models;

public class SystemRole { [Key] public int RoleID { get; set; } public string RoleName { get; set; } = ""; }
public class AppUser { [Key] public int UserID { get; set; } public int RoleID { get; set; } public string FirstName { get; set; } = ""; public string LastName { get; set; } = ""; public string Email { get; set; } = ""; public string PasswordHash { get; set; } = ""; public string? ProfileImageUrl { get; set; } [ForeignKey("RoleID")] public SystemRole? Role { get; set; } }
public class Event { [Key] public int EventID { get; set; } public int OrganiserID { get; set; } public string Name { get; set; } = ""; public string Description { get; set; } = ""; public DateTime EventDate { get; set; } public string Location { get; set; } = ""; public decimal Distance { get; set; } public string EventType { get; set; } = ""; public string? BannerImageUrl { get; set; } [ForeignKey("OrganiserID")] public AppUser? Organiser { get; set; } }
