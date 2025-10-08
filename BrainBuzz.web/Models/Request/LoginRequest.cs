using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Models.Request
{
    /// <summary>
    /// Login request model with validation
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username or email is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9@._-]+$", ErrorMessage = "Username can only contain letters, numbers, @, ., _, and -")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Remember me option
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }
}
