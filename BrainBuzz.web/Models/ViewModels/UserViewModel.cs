namespace BrainBuzz.web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for User data transfer
    /// </summary>
    public class UserViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Additional UI-specific properties
        public string DisplayName => !string.IsNullOrEmpty(FullName) ? FullName : Username;
        public string Initials => GetInitials(DisplayName);
        public string LastLoginDisplay => LastLoginDate?.ToString("MMM dd, yyyy") ?? "Never";
        
        private static string GetInitials(string name)
        {
            if (string.IsNullOrEmpty(name)) return "U";
            
            var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length >= 2)
                return $"{words[0][0]}{words[1][0]}".ToUpper();
            
            return name.Length >= 2 ? name.Substring(0, 2).ToUpper() : name.ToUpper();
        }
    }
}
