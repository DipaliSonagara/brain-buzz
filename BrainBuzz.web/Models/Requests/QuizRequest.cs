using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Models.Requests
{
    /// <summary>
    /// Request model for Quiz creation and updates with validation
    /// </summary>
    public class QuizRequest
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Quiz title is required")]
        [StringLength(200, ErrorMessage = "Quiz title cannot exceed 200 characters")]
        [Display(Name = "Quiz Title")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Category is required")]
        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;
        
        [Range(1, 300, ErrorMessage = "Time limit must be between 1 and 300 minutes")]
        [Display(Name = "Time Limit (minutes)")]
        public int? TimeLimitMinutes { get; set; }
        
        [Display(Name = "Active Quiz")]
        public bool IsActive { get; set; } = true;
        
        [Required(ErrorMessage = "At least one question is required")]
        [MinLength(1, ErrorMessage = "At least one question is required")]
        public List<QuestionRequest> Questions { get; set; } = new();
    }
}
