using BrainBuzz.web.Models.Request;

namespace BrainBuzz.web.Services.Interface
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates user with proper credential validation
        /// </summary>
        /// <param name="username">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Authentication result</returns>
        Task<AuthenticationResult> AuthenticateAsync(string username, string password);
        
        /// <summary>
        /// Registers a new user with proper validation
        /// </summary>
        /// <param name="registerRequest">Registration request</param>
        /// <returns>Registration result</returns>
        Task<RegistrationResult> RegisterAsync(RegisterRequest registerRequest);
        
        /// <summary>
        /// Checks if user is currently authenticated
        /// </summary>
        /// <returns>Authentication status</returns>
        Task<AuthenticationResult> CheckAuthenticationAsync();
        
        /// <summary>
        /// Creates a secure session for authenticated user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="username">Username</param>
        /// <returns>Session ID</returns>
        Task<string> CreateSessionAsync(string userId, string username);
        
        /// <summary>
        /// Clears user session and logs out
        /// </summary>
        /// <returns>Task</returns>
        Task LogoutAsync();
        
        /// <summary>
        /// Validates user session
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>True if session is valid</returns>
        Task<bool> ValidateSessionAsync(string sessionId);
    }
}
