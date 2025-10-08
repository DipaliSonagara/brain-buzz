namespace BrainBuzz.web.Constants
{
    /// <summary>
    /// Application role constants
    /// </summary>
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        
        public static string[] AllRoles => new[] { Admin, Customer };
    }
}

