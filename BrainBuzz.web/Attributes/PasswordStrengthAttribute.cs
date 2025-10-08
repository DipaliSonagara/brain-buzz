using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Attributes
{
    /// <summary>
    /// Custom validation attribute for password strength
    /// </summary>
    public class PasswordStrengthAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string password)
                return false;

            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Minimum length check
            if (password.Length < 6)
            {
                ErrorMessage = "Password must be at least 6 characters long";
                return false;
            }

            // Maximum length check
            if (password.Length > 100)
            {
                ErrorMessage = "Password is too long (maximum 100 characters)";
                return false;
            }

            // Check for common weak passwords
            var weakPasswords = new[] { "password", "123456", "qwerty", "abc123", "password123", "admin", "letmein" };
            if (weakPasswords.Contains(password.ToLower()))
            {
                ErrorMessage = "Password is too common. Please choose a stronger password";
                return false;
            }

            return true;
        }
    }
}

