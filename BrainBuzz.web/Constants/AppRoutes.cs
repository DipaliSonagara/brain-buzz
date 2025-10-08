namespace BrainBuzz.web.Constants
{
    /// <summary>
    /// Application route constants
    /// </summary>
    public static class AppRoutes
    {
        // Authentication routes
        public const string Login = "/login";
        public const string Register = "/register";
        public const string Logout = "/logout";
        
        // Customer routes
        public const string Home = "/";
        public const string Overview = "/overview";
        public const string Quizzes = "/quizzes";
        public const string Quiz = "/quiz";
        public const string Results = "/results";
        public const string Profile = "/profile";
        public const string Unauthorized = "/unauthorized";
        public const string TestSeeding = "/test-seeding";
        public const string ManualSeed = "/manual-seed";
        
        // Admin routes
        public const string AdminDashboard = "/admin/dashboard";
        public const string AdminQuizzes = "/admin/quizzes";
        public const string AdminQuizzesNew = "/admin/quizzes/new";
        public const string AdminQuizzesEdit = "/admin/quizzes/edit";
        public const string AdminCategories = "/admin/categories";
        public const string AdminCategoriesNew = "/admin/categories/new";
        public const string AdminUsers = "/admin/users";
        public const string AdminAuditLogs = "/admin/audit-logs";
        public const string AdminSettings = "/admin/settings";
        
        // Admin route with query parameters
        public static string AdminQuizzesWithFilter(bool? activeFilter = null)
        {
            if (activeFilter.HasValue)
            {
                return $"{AdminQuizzes}?active={activeFilter.Value.ToString().ToLower()}";
            }
            return AdminQuizzes;
        }
        
        public static string AdminQuizzesEditWithId(int quizId)
        {
            return $"{AdminQuizzesEdit}?id={quizId}";
        }
    }
}
