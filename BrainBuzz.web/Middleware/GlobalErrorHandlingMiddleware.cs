using System.Net;
using System.Text.Json;

namespace BrainBuzz.web.Middleware
{
    /// <summary>
    /// Global error handling middleware
    /// </summary>
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                Success = false,
                Message = GetUserFriendlyMessage(exception),
                Details = context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true 
                    ? exception.ToString() 
                    : null,
                Timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                TimeoutException => (int)HttpStatusCode.RequestTimeout,
                InvalidOperationException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private static string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => "Required information is missing. Please fill in all required fields.",
                ArgumentException => "Invalid input provided. Please check your data and try again.",
                UnauthorizedAccessException => "You don't have permission to perform this action.",
                TimeoutException => "The operation timed out. Please try again.",
                InvalidOperationException => "The operation cannot be completed at this time. Please try again later.",
                _ => "An unexpected error occurred. Please try again or contact support if the problem persists."
            };
        }
    }

    /// <summary>
    /// Extension method to register the middleware
    /// </summary>
    public static class GlobalErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }
}
