using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Models.Requests
{
    /// <summary>
    /// Request model for Question creation and updates with validation
    /// </summary>
    public class QuestionRequest
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        
        [Required(ErrorMessage = "Question text is required")]
        [StringLength(500, ErrorMessage = "Question text cannot exceed 500 characters")]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Question type is required")]
        [Display(Name = "Question Type")]
        public string Type { get; set; } = "MultipleChoice";
        
        [Required(ErrorMessage = "At least 2 options are required")]
        [MinLength(2, ErrorMessage = "At least 2 options are required")]
        [Display(Name = "Options")]
        public List<string> Options { get; set; } = new();
        
        [Required(ErrorMessage = "Correct answer is required")]
        [Display(Name = "Correct Answer")]
        public string CorrectAnswer { get; set; } = string.Empty;
        
        [StringLength(1000, ErrorMessage = "Explanation cannot exceed 1000 characters")]
        [Display(Name = "Explanation")]
        public string Explanation { get; set; } = string.Empty;
        
        public int QuestionOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
