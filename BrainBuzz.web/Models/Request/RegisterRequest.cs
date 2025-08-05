using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Models.Request
{
     public class RegisterRequest
     {
          [Required(ErrorMessage = "Username must be pass")]
          public string Username { get; set; } = string.Empty;
          [Required]
          public string Password { get; set; } = string.Empty;
          [Required]
          public string ConfirmPassword { get; set; } = string.Empty;
          public string? PasswordHash { get; set; }
          public string? Role { get; set; }
          [Required]
          //public string FirstName { get; set; } = string.Empty;
          //[Required]
          //public string LastName { get; set; } = string.Empty;
          public string? Email { get; set; }
     }
}
