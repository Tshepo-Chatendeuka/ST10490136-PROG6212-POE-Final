using System.ComponentModel.DataAnnotations;
namespace RaceDay.Mvc.ViewModels;
public class LoginVm { [Required, EmailAddress] public string Email { get; set; } = ""; [Required, DataType(DataType.Password)] public string Password { get; set; } = ""; }
public class RegisterVm { [Required] public string FirstName { get; set; } = ""; [Required] public string LastName { get; set; } = ""; [Required, EmailAddress] public string Email { get; set; } = ""; [Required, DataType(DataType.Password)] public string Password { get; set; } = ""; [Required] public string Role { get; set; } = "Participant"; }
