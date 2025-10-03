namespace BrainBuzz.web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Quiz data transfer
    /// </summary>
    public class QuizViewModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TotalQuestions { get; set; }
        public int TimeLimit { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        // Additional UI-specific properties
        public string TimeLimitDisplay => $"{TimeLimit} min";
        public string DifficultyBadgeClass => Difficulty.ToLower() switch
        {
            "easy" => "badge-success",
            "medium" => "badge-warning", 
            "hard" => "badge-danger",
            _ => "badge-secondary"
        };
        public string CategoryIcon => Category.ToLower() switch
        {
            "science" => "fas fa-flask",
            "history" => "fas fa-landmark",
            "literature" => "fas fa-book",
            "technology" => "fas fa-laptop-code",
            "sports" => "fas fa-football-ball",
            "general" => "fas fa-question-circle",
            _ => "fas fa-puzzle-piece"
        };
    }
}
