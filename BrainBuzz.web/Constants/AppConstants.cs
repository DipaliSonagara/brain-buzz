namespace BrainBuzz.web.Constants
{
    /// <summary>
    /// Application constants
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// Local storage keys
        /// </summary>
        public static class StorageKeys
        {
            public const string SessionId = "sessionId";
            public const string Username = "username";
        }

        /// <summary>
        /// Route paths
        /// </summary>
        public static class Routes
        {
            public const string Home = "/";
            public const string Login = "/login";
            public const string Register = "/register";
            public const string Overview = "/overview";
            public const string Quizzes = "/quizzes";
            public const string Results = "/results";
            public const string Profile = "/profile";
        }

        /// <summary>
        /// Error messages
        /// </summary>
        public static class ErrorMessages
        {
            public const string InvalidCredentials = "Invalid username or password. Please try again.";
            public const string NetworkError = "Network error. Please check your connection and try again.";
            public const string TimeoutError = "Request timeout. Please try again.";
            public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
        }

        /// <summary>
        /// Success messages
        /// </summary>
        public static class SuccessMessages
        {
            public const string LoginSuccess = "Login successful! Redirecting...";
            public const string RegistrationSuccess = "Registration successful! Redirecting...";
        }

        /// <summary>
        /// UI text
        /// </summary>
        public static class UIText
        {
            public const string Loading = "Loading...";
            public const string CheckingAuthentication = "Checking authentication...";
            public const string LoadingDashboard = "Loading dashboard...";
            public const string LoadingQuizzes = "Loading quizzes...";
            public const string Welcome = "Welcome, {0}!";
        }
    }
}
