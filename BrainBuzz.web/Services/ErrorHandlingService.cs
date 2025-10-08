using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Services
{
    /// <summary>
    /// Service for handling errors and providing user-friendly messages
    /// </summary>
    public class ErrorHandlingService
    {
        /// <summary>
        /// Gets user-friendly error message from exception
        /// </summary>
        public static string GetUserFriendlyErrorMessage(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => "Required information is missing. Please fill in all required fields.",
                ArgumentException => "Invalid input provided. Please check your data and try again.",
                ValidationException => "The provided data is not valid. Please check your input and try again.",
                UnauthorizedAccessException => "You don't have permission to perform this action.",
                TimeoutException => "The operation timed out. Please try again.",
                InvalidOperationException => "The operation cannot be completed at this time. Please try again later.",
                _ => "An unexpected error occurred. Please try again or contact support if the problem persists."
            };
        }

        /// <summary>
        /// Gets validation error messages from model state
        /// </summary>
        public static List<string> GetValidationErrors(object model)
        {
            var errors = new List<string>();
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, context, results, true))
            {
                errors.AddRange(results.Select(r => r.ErrorMessage ?? "Validation error"));
            }

            return errors;
        }

        /// <summary>
        /// Logs error with appropriate level
        /// </summary>
        public static void LogError(ILogger logger, Exception exception, string message, params object[] args)
        {
            switch (exception)
            {
                case ArgumentNullException:
                case ArgumentException:
                case ValidationException:
                case UnauthorizedAccessException:
                    logger.LogWarning(exception, message, args);
                    break;
                case TimeoutException:
                    logger.LogError(exception, message, args);
                    break;
                default:
                    logger.LogError(exception, message, args);
                    break;
            }
        }

        /// <summary>
        /// Creates a standardized error response
        /// </summary>
        public static object CreateErrorResponse(string message, string? details = null, string? errorCode = null)
        {
            return new
            {
                Success = false,
                Message = message,
                Details = details,
                ErrorCode = errorCode,
                Timestamp = DateTime.UtcNow
            };
        }

    }
}
