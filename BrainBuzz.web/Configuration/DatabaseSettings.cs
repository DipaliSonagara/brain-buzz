namespace BrainBuzz.web.Configuration
{
    /// <summary>
    /// Database configuration settings
    /// </summary>
    public class DatabaseSettings
    {
        public const string SectionName = "Database";
        
        /// <summary>
        /// Database connection string
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;
        
        /// <summary>
        /// Command timeout in seconds
        /// </summary>
        public int CommandTimeout { get; set; } = 30;
        
        /// <summary>
        /// Enable sensitive data logging (only for development)
        /// </summary>
        public bool EnableSensitiveDataLogging { get; set; } = false;
    }
}
