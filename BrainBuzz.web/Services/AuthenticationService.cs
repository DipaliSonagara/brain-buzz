using BrainBuzz.web.Services.Interface;
using BrainBuzz.web.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Microsoft.Extensions.Options;
using System.Text.Json;
using BrainBuzz.web.Models.Request;

namespace BrainBuzz.web.Services
{
    /// <summary>
    /// Authentication service with proper security
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly SessionService _sessionService;
        private readonly IJSRuntime _jsRuntime;
        private readonly SecuritySettings _securitySettings;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ILoadingService _loadingService;

        public AuthenticationService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            SessionService sessionService,
            IJSRuntime jsRuntime,
            IOptions<SecuritySettings> securitySettings,
            ILogger<AuthenticationService> logger,
            ILoadingService loadingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sessionService = sessionService;
            _jsRuntime = jsRuntime;
            _securitySettings = securitySettings.Value;
            _logger = logger;
            _loadingService = loadingService;
        }

        /// <summary>
        /// Authenticates user with proper credential validation
        /// </summary>
        public async Task<AuthenticationResult> AuthenticateAsync(string username, string password)
        {
            _loadingService.StartLoading("Signing you in...");
            
            try
            {
                _logger.LogInformation("Authentication attempt for user: {Username}", username);

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    _logger.LogWarning("Authentication failed: Empty username or password");
                    _loadingService.StopLoading();
                    return new AuthenticationResult
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Username and password are required"
                    };
                }

                // Find user by username or email
                var user = await _userManager.FindByNameAsync(username) 
                          ?? await _userManager.FindByEmailAsync(username);

                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: User not found - {Username}", username);
                    return new AuthenticationResult
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Invalid username or password"
                    };
                }

                // Check if user is locked out
                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarning("Authentication failed: User locked out - {Username}", username);
                    return new AuthenticationResult
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Account is temporarily locked. Please try again later."
                    };
                }

                // Verify password
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("Authentication successful for user: {Username}", username);
                    
                    _loadingService.UpdateLoading("Creating your session...", 75);
                    var sessionId = await CreateSessionAsync(user.Id, user.UserName ?? string.Empty);
                    
                    _loadingService.StopLoading();
                    return new AuthenticationResult
                    {
                        IsAuthenticated = true,
                        Username = user.UserName ?? string.Empty,
                        SessionId = sessionId,
                        UserId = user.Id
                    };
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogWarning("Authentication failed: Account locked out - {Username}", username);
                    _loadingService.StopLoading();
                    return new AuthenticationResult
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Account is temporarily locked due to multiple failed attempts."
                    };
                }
                else
                {
                    _logger.LogWarning("Authentication failed: Invalid password - {Username}", username);
                    _loadingService.StopLoading();
                    return new AuthenticationResult
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Invalid username or password"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authentication error for user: {Username}", username);
                _loadingService.StopLoading();
                return new AuthenticationResult
                {
                    IsAuthenticated = false,
                    ErrorMessage = "An error occurred during authentication. Please try again."
                };
            }
        }

        /// <summary>
        /// Registers a new user with proper validation
        /// </summary>
        public async Task<RegistrationResult> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {
                _logger.LogInformation("Registration attempt for user: {Username}", registerRequest.Username);

                // Validate required fields
                if (string.IsNullOrWhiteSpace(registerRequest.Username) ||
                    string.IsNullOrWhiteSpace(registerRequest.Email) ||
                    string.IsNullOrWhiteSpace(registerRequest.Password))
                {
                    return new RegistrationResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "All fields are required"
                    };
                }

                // Check if user already exists
                var existingUser = await _userManager.FindByNameAsync(registerRequest.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: Username already exists - {Username}", registerRequest.Username);
                    return new RegistrationResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Username already exists"
                    };
                }

                existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: Email already exists - {Email}", registerRequest.Email);
                    return new RegistrationResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Email address is already registered"
                    };
                }

                // Create new user
                var user = new IdentityUser
                {
                    UserName = registerRequest.Username,
                    Email = registerRequest.Email,
                    EmailConfirmed = false // Set to true if you want to skip email confirmation
                };

                var result = await _userManager.CreateAsync(user, registerRequest.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully: {Username}", registerRequest.Username);
                return new RegistrationResult
                {
                    IsSuccess = true,
                    Message = "Registration successful"
                };
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Registration failed for {Username}: {Errors}", registerRequest.Username, errors);
                    return new RegistrationResult
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Registration failed: {errors}"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error for user: {Username}", registerRequest.Username);
                return new RegistrationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "An error occurred during registration. Please try again."
                };
            }
        }

        /// <summary>
        /// Checks if user is currently authenticated
        /// </summary>
        public async Task<AuthenticationResult> CheckAuthenticationAsync()
        {
            try
            {
                var sessionId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "sessionId") ?? string.Empty;
                var username = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "username") ?? string.Empty;
                var userId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId") ?? string.Empty;

                _logger.LogInformation("CheckAuthentication: SessionId='{SessionId}', Username='{Username}', UserId='{UserId}'", 
                    sessionId, username, userId);
                
                // Also log to console for debugging
                Console.WriteLine($"CheckAuthentication: SessionId='{sessionId}', Username='{username}', UserId='{userId}'");
                
                // Debug: Check what's actually in localStorage
                try
                {
                    var allKeys = await _jsRuntime.InvokeAsync<string>("eval", "Object.keys(localStorage)");
                    Console.WriteLine($"CheckAuthentication: All localStorage keys: {allKeys}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CheckAuthentication: Error getting localStorage keys: {ex.Message}");
                }

                if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation("CheckAuthentication: Missing session data, returning not authenticated");
                    return new AuthenticationResult { IsAuthenticated = false };
                }

                var isValid = await ValidateSessionAsync(sessionId);
                _logger.LogInformation("CheckAuthentication: Session validation result: {IsValid}", isValid);
                
                if (isValid)
                {
                    _logger.LogInformation("CheckAuthentication: User authenticated successfully");
                    return new AuthenticationResult
                    {
                        IsAuthenticated = true,
                        Username = username,
                        SessionId = sessionId,
                        UserId = userId
                    };
                }

                _logger.LogInformation("CheckAuthentication: Session invalid, returning not authenticated");
                return new AuthenticationResult { IsAuthenticated = false };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking authentication status");
                return new AuthenticationResult { IsAuthenticated = false };
            }
        }

        /// <summary>
        /// Creates a secure session for authenticated user
        /// </summary>
        public async Task<string> CreateSessionAsync(string userId, string username)
        {
            try
            {
                var sessionId = Guid.NewGuid().ToString();
                
                _sessionService.CreateSession(sessionId, username);
                
                // Store session data in browser
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "sessionId", sessionId);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "username", username);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userId", userId);
                
                _logger.LogInformation("Session created for user: {Username}, SessionId: {SessionId}", username, sessionId);
                
                // Also log to console for debugging
                Console.WriteLine($"Session created for user: {username}, SessionId: {sessionId}");
                
                // Debug: Verify the data was stored
                try
                {
                    var storedSessionId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "sessionId");
                    var storedUsername = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "username");
                    var storedUserId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId");
                    Console.WriteLine($"Session verification - Stored: SessionId='{storedSessionId}', Username='{storedUsername}', UserId='{storedUserId}'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Session verification failed: {ex.Message}");
                }
                
                return sessionId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating session for user: {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// Clears user session and logs out
        /// </summary>
        public async Task LogoutAsync()
        {
            try
            {
                var sessionId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "sessionId");
                var username = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "username");
                if (!string.IsNullOrEmpty(sessionId))
                {
                    _sessionService.RemoveSession(sessionId);
                }

                await _signInManager.SignOutAsync();
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "sessionId");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "username");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userId");

                _logger.LogInformation("User '{Username}' logged out successfully", username ?? "Unknown");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                throw;
            }
        }

        /// <summary>
        /// Validates user session
        /// </summary>
        public async Task<bool> ValidateSessionAsync(string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    return false;
                }

                return _sessionService.IsUserLoggedIn(sessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating session: {SessionId}", sessionId);
                return false;
            }
        }
    }

     /// <summary>
    /// Enhanced authentication result
     /// </summary>
    public class AuthenticationResult
    {
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; } = string.Empty;
          public string SessionId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
          public string ErrorMessage { get; set; } = string.Empty;
     }

     /// <summary>
    /// Enhanced registration result
     /// </summary>
    public class RegistrationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
