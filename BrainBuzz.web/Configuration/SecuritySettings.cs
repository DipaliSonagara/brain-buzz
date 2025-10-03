namespace BrainBuzz.web.Configuration
{
    /// <summary>
    /// Security configuration settings
    /// </summary>
    public class SecuritySettings
    {
        public const string SectionName = "Security";
        
        /// <summary>
        /// JWT secret key for token generation
        /// </summary>
        public string JwtSecretKey { get; set; } = string.Empty;
        
        /// <summary>
        /// JWT issuer
        /// </summary>
        public string JwtIssuer { get; set; } = string.Empty;
        
        /// <summary>
        /// JWT audience
        /// </summary>
        public string JwtAudience { get; set; } = string.Empty;
        
        /// <summary>
        /// JWT expiration time in minutes
        /// </summary>
        public int JwtExpirationMinutes { get; set; } = 60;
        
        /// <summary>
        /// Session timeout in minutes
        /// </summary>
        public int SessionTimeoutMinutes { get; set; } = 30;
        
        /// <summary>
        /// Enable HTTPS redirection
        /// </summary>
        public bool RequireHttps { get; set; } = true;
        
        /// <summary>
        /// Password requirements
        /// </summary>
        public PasswordRequirements PasswordRequirements { get; set; } = new();
    }
    
    /// <summary>
    /// Password requirements configuration
    /// </summary>
    public class PasswordRequirements
    {
        /// <summary>
        /// Minimum password length
        /// </summary>
        public int MinimumLength { get; set; } = 8;
        
        /// <summary>
        /// Require uppercase letters
        /// </summary>
        public bool RequireUppercase { get; set; } = true;
        
        /// <summary>
        /// Require lowercase letters
        /// </summary>
        public bool RequireLowercase { get; set; } = true;
        
        /// <summary>
        /// Require digits
        /// </summary>
        public bool RequireDigit { get; set; } = true;
        
        /// <summary>
        /// Require special characters
        /// </summary>
        public bool RequireNonAlphanumeric { get; set; } = true;
        
        /// <summary>
        /// Required unique characters
        /// </summary>
        public int RequiredUniqueChars { get; set; } = 1;
    }
}
