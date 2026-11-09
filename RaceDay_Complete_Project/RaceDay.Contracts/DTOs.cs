namespace RaceDay.Contracts;

public class AuthResponse { public int UserId { get; set; } public string Role { get; set; } = string.Empty; public string? FirstName { get; set; } public string? Email { get; set; } }
public class RegisterRequest { public string FirstName { get; set; } = ""; public string LastName { get; set; } = ""; public string Email { get; set; } = ""; public string Password { get; set; } = ""; public string Role { get; set; } = ""; }
public class LoginRequest { public string Email { get; set; } = ""; public string Password { get; set; } = ""; }
public class EventDto { public int EventId { get; set; } public string Name { get; set; } = ""; public string Description { get; set; } = ""; public DateTime EventDate { get; set; } public string Location { get; set; } = ""; public decimal DistanceKm { get; set; } public string EventType { get; set; } = ""; public string? BannerImageUrl { get; set; } }
public class CreateEventRequest { public string Name { get; set; } = ""; public string Description { get; set; } = ""; public DateTime EventDate { get; set; } public string Location { get; set; } = ""; public decimal DistanceKm { get; set; } public string EventType { get; set; } = ""; }
public class CreateEnrolmentRequest { public int EventId { get; set; } public int CategoryId { get; set; } }
