namespace BrainBuzz.web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Quiz data transfer and display
    /// </summary>
    public class QuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int? TimeLimitMinutes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
        
        // Legacy properties for backward compatibility
        public int QuizId { get => Id; set => Id = value; }
        public string QuizName { get => Title; set => Title = value; }
        public int TotalQuestions { get => Questions.Count; set { } }
        public int TimeLimit { get => TimeLimitMinutes ?? 0; set => TimeLimitMinutes = value; }
        public string Difficulty { get; set; } = string.Empty;
        public DateTime CreatedDate { get => CreatedAt; set => CreatedAt = value; }
        public DateTime? UpdatedDate { get => UpdatedAt; set => UpdatedAt = value; }
        
        // Additional UI-specific properties
        public string TimeLimitDisplay => TimeLimitMinutes.HasValue ? $"{TimeLimitMinutes} min" : "No limit";
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
