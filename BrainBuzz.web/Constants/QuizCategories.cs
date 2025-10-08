namespace BrainBuzz.web.Constants
{
    /// <summary>
    /// Predefined quiz categories
    /// </summary>
    public static class QuizCategories
    {
        public static readonly List<string> Categories = new()
        {
            "General Knowledge",
            "Science",
            "Mathematics",
            "History",
            "Geography",
            "Literature",
            "Technology",
            "Sports",
            "Entertainment",
            "Health & Medicine",
            "Business & Finance",
            "Art & Culture",
            "Language",
            "Philosophy",
            "Psychology",
            "Education",
            "Environment",
            "Food & Cooking",
            "Travel",
            "Music"
        };

        /// <summary>
        /// Gets all categories as a list
        /// </summary>
        public static List<string> GetAllCategories()
        {
            return Categories.ToList();
        }

        /// <summary>
        /// Checks if a category is valid
        /// </summary>
        public static bool IsValidCategory(string category)
        {
            return Categories.Contains(category, StringComparer.OrdinalIgnoreCase);
        }
    }
}
