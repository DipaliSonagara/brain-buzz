using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BrainBuzz.web.Services
{
    /// <summary>
    /// Service for handling custom validation logic
    /// </summary>
    public class ValidationService
    {
        /// <summary>
        /// Validates password strength
        /// </summary>
        public static ValidationResult ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return new ValidationResult("Password is required");

            if (password.Length < 6)
                return new ValidationResult("Password must be at least 6 characters long");

            if (password.Length > 100)
                return new ValidationResult("Password is too long (maximum 100 characters)");

            // Check for common weak passwords
            var weakPasswords = new[] { "password", "123456", "qwerty", "abc123", "password123" };
            if (weakPasswords.Contains(password.ToLower()))
                return new ValidationResult("Password is too common. Please choose a stronger password");

            return ValidationResult.Success!;
        }

        /// <summary>
        /// Validates username format
        /// </summary>
        public static ValidationResult ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new ValidationResult("Username is required");

            if (username.Length < 3)
                return new ValidationResult("Username must be at least 3 characters long");

            if (username.Length > 50)
                return new ValidationResult("Username is too long (maximum 50 characters)");

            // Check for valid characters
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_-]+$"))
                return new ValidationResult("Username can only contain letters, numbers, underscore, and dash");

            // Check for reserved usernames
            var reservedUsernames = new[] { "admin", "administrator", "root", "user", "guest", "test", "api", "www", "mail", "support" };
            if (reservedUsernames.Contains(username.ToLower()))
                return new ValidationResult("This username is reserved and cannot be used");

            return ValidationResult.Success!;
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        public static ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult("Email is required");

            if (email.Length > 256)
                return new ValidationResult("Email address is too long");

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    return new ValidationResult("Invalid email format");
            }
            catch
            {
                return new ValidationResult("Invalid email format");
            }

            return ValidationResult.Success!;
        }


        /// <summary>
        /// Validates quiz data
        /// </summary>
        public static ValidationResult ValidateQuizData(string quizName, string description, int timeLimit, int totalQuestions)
        {
            if (string.IsNullOrWhiteSpace(quizName))
                return new ValidationResult("Quiz name is required");

            if (quizName.Length > 200)
                return new ValidationResult("Quiz name is too long (maximum 200 characters)");

            if (string.IsNullOrWhiteSpace(description))
                return new ValidationResult("Quiz description is required");

            if (description.Length > 1000)
                return new ValidationResult("Quiz description is too long (maximum 1000 characters)");

            if (timeLimit < 1 || timeLimit > 120)
                return new ValidationResult("Time limit must be between 1 and 120 minutes");

            if (totalQuestions < 1 || totalQuestions > 50)
                return new ValidationResult("Total questions must be between 1 and 50");

            return ValidationResult.Success!;
        }

        /// <summary>
        /// Validates question data
        /// </summary>
        public static ValidationResult ValidateQuestionData(string questionText, string optionA, string optionB, string optionC, string optionD, string correctAnswer)
        {
            if (string.IsNullOrWhiteSpace(questionText))
                return new ValidationResult("Question text is required");

            if (questionText.Length > 1000)
                return new ValidationResult("Question text is too long (maximum 1000 characters)");

            var options = new[] { optionA, optionB, optionC, optionD };
            for (int i = 0; i < options.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(options[i]))
                    return new ValidationResult($"Option {(char)('A' + i)} is required");

                if (options[i].Length > 500)
                    return new ValidationResult($"Option {(char)('A' + i)} is too long (maximum 500 characters)");
            }

            if (string.IsNullOrWhiteSpace(correctAnswer))
                return new ValidationResult("Correct answer is required");

            if (!new[] { "A", "B", "C", "D" }.Contains(correctAnswer.ToUpper()))
                return new ValidationResult("Correct answer must be A, B, C, or D");

            return ValidationResult.Success!;
        }
    }
}
