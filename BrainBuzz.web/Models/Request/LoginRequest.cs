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
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Remember me option
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }
}
