using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Models.Request
{
     public class RegisterRequest
     {
          [Required(ErrorMessage = "Username must be pass")]
          public string Username { get; set; } = default!;
          [Required]
          public string Password { get; set; } = default!;
          [Required]
          public string ConfirmPassword { get; set; } = default!;
          [Required]
          public string PasswordHash { get; set; } = default!;
          public string? Role { get; set; }
          [Required]
          public string FirstName { get; set; } = default!;
          [Required]
          public string LastName { get; set; } = default!;
          public string? Email { get; set; }
     }
}
