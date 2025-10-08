using System.ComponentModel.DataAnnotations;
using BrainBuzz.web.Attributes;

namespace BrainBuzz.web.Models.Request
{
     public class RegisterRequest
     {
          [Required(ErrorMessage = "Username is required")]
          [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
          [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscore, and dash")]
          public string Username { get; set; } = string.Empty;
          
          [Required(ErrorMessage = "Email is required")]
          [EmailAddress(ErrorMessage = "Please enter a valid email address")]
          [StringLength(256, ErrorMessage = "Email address is too long")]
          public string Email { get; set; } = string.Empty;
          
          [Required(ErrorMessage = "Full name is required")]
          [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
          [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Full name can only contain letters, spaces, apostrophes, and hyphens")]
          public string FullName { get; set; } = string.Empty;
          
          [Required(ErrorMessage = "Password is required")]
          [PasswordStrength]
          [DataType(DataType.Password)]
          public string Password { get; set; } = string.Empty;
          
          [Required(ErrorMessage = "Please confirm your password")]
          [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
          [DataType(DataType.Password)]
          public string ConfirmPassword { get; set; } = string.Empty;
          
          public string? PasswordHash { get; set; }
          public string? Role { get; set; }
     }
}
