namespace BrainBuzz.web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Question data transfer
    /// </summary>
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public int QuestionOrder { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Additional UI-specific properties
        public List<string> Options => new List<string> { OptionA, OptionB, OptionC, OptionD };
        public string CorrectAnswerDisplay => $"Correct Answer: {CorrectAnswer}";
    }
}
