using BrainBuzz.web.Services.Interface;
using Microsoft.JSInterop;

namespace BrainBuzz.web.Services
{
    /// <summary>
    /// Service for handling authentication-related operations
    /// </summary>
    public class AuthenticationService
    {
        private readonly SessionService _sessionService;
        private readonly IJSRuntime _jsRuntime;

        public AuthenticationService(SessionService sessionService, IJSRuntime jsRuntime)
        {
            _sessionService = sessionService;
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Checks if the current user is authenticated
        /// </summary>
        /// <returns>Authentication result with user information</returns>
        public async Task<AuthenticationResult> CheckAuthenticationAsync()
        {
            try
            {
                Console.WriteLine("AuthenticationService: Starting authentication check...");
                
                var sessionId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "sessionId") ?? string.Empty;
                var username = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "username") ?? string.Empty;
                
                Console.WriteLine($"AuthenticationService: SessionId: '{sessionId}', Username: '{username}'");

                if (!string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(username))
                {
                    var isAuthenticated = _sessionService.IsUserLoggedIn(sessionId);
                    Console.WriteLine($"AuthenticationService: IsUserLoggedIn result: {isAuthenticated}");
                    
                    return new AuthenticationResult
                    {
                        IsAuthenticated = isAuthenticated,
                        Username = isAuthenticated ? username : string.Empty,
                        SessionId = isAuthenticated ? sessionId : string.Empty
                    };
                }

                Console.WriteLine("AuthenticationService: No session found, returning not authenticated");
                return new AuthenticationResult { IsAuthenticated = false };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AuthenticationService: Exception during authentication check: {ex.Message}");
                return new AuthenticationResult { IsAuthenticated = false };
            }
        }

        /// <summary>
        /// Clears authentication data from browser storage
        /// </summary>
        public async Task ClearAuthenticationAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "sessionId");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "username");
            }
            catch (Exception)
            {
                // Silently handle errors
            }
        }

        /// <summary>
        /// Creates a new authentication session
        /// </summary>
        /// <param name="username">The username to create session for</param>
        /// <returns>The created session ID</returns>
        public async Task<string> CreateSessionAsync(string username)
        {
            var sessionId = Guid.NewGuid().ToString();
            Console.WriteLine($"AuthenticationService.CreateSessionAsync: Creating session for user: {username}, sessionId: {sessionId}");
            
            _sessionService.CreateSession(sessionId, username);
            Console.WriteLine("AuthenticationService.CreateSessionAsync: Session created in SessionService");

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "sessionId", sessionId);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "username", username);
            Console.WriteLine("AuthenticationService.CreateSessionAsync: Session data stored in localStorage");

            return sessionId;
        }

        /// <summary>
        /// Handles user login
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Login result</returns>
        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            try
            {
                Console.WriteLine($"AuthenticationService.LoginAsync: Starting login for user: {username}");
                
                // Simple validation - in a real app, you'd validate against a database
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("AuthenticationService.LoginAsync: Validation failed - empty username or password");
                    return new LoginResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Username and password are required"
                    };
                }

                // For demo purposes, accept any non-empty username/password
                // In a real application, you would validate against a database
                Console.WriteLine("AuthenticationService.LoginAsync: Creating session...");
                var sessionId = await CreateSessionAsync(username);
                Console.WriteLine($"AuthenticationService.LoginAsync: Session created with ID: {sessionId}");
                
                return new LoginResult
                {
                    IsSuccess = true,
                    SessionId = sessionId,
                    Username = username
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AuthenticationService.LoginAsync: Exception occurred: {ex.Message}");
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "An error occurred during login"
                };
            }
        }

        /// <summary>
        /// Handles user registration
        /// </summary>
        /// <param name="registerRequest">Registration request</param>
        /// <returns>Registration result</returns>
        public async Task<RegistrationResult> RegisterAsync(Models.Request.RegisterRequest registerRequest)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(registerRequest.Username) || 
                    string.IsNullOrEmpty(registerRequest.Email) ||
                    string.IsNullOrEmpty(registerRequest.Password))
                {
                    return new RegistrationResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "All fields are required"
                    };
                }

                // Validate email format
                if (!registerRequest.Email.Contains("@"))
                {
                    return new RegistrationResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Please enter a valid email address"
                    };
                }

                // For demo purposes, accept any valid registration
                // In a real application, you would save to database and check for duplicates
                
                return new RegistrationResult
                {
                    IsSuccess = true,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new RegistrationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "An error occurred during registration"
                };
            }
        }
    }

     /// <summary>
     /// Result of authentication check
     /// </summary>
     //public class AuthenticationResult
     //{
     //    public bool IsAuthenticated { get; set; }
     //    public string Username { get; set; } = string.Empty;
     //    public string SessionId { get; set; } = string.Empty;
     //}

     ///// <summary>
     ///// Result of login operation
     ///// </summary>
     public class LoginResult
     {
          public bool IsSuccess { get; set; }
          public string SessionId { get; set; } = string.Empty;
          public string Username { get; set; } = string.Empty;
          public string ErrorMessage { get; set; } = string.Empty;
     }

     /// <summary>
     /// Result of registration operation
     /// </summary>
     //public class RegistrationResult
     //{
     //    public bool IsSuccess { get; set; }
     //    public string Message { get; set; } = string.Empty;
     //    public string ErrorMessage { get; set; } = string.Empty;
     //}
}
