using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Models.Request
{
     public class RegisterRequest
     {
          [Required(ErrorMessage = "Username is required")]
          [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
          public string Username { get; set; } = string.Empty;
          
          [Required(ErrorMessage = "Email is required")]
          [EmailAddress(ErrorMessage = "Please enter a valid email address")]
          public string Email { get; set; } = string.Empty;
          
          [Required(ErrorMessage = "Full name is required")]
          [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
          public string FullName { get; set; } = string.Empty;
          
          [Required(ErrorMessage = "Password is required")]
          [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
          [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
               ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
          public string Password { get; set; } = string.Empty;
          
          [Required(ErrorMessage = "Please confirm your password")]
          [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
          public string ConfirmPassword { get; set; } = string.Empty;
          
          public string? PasswordHash { get; set; }
          public string? Role { get; set; }
     }
}
