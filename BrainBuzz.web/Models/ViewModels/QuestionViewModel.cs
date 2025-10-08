namespace BrainBuzz.web.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Question data transfer and display
    /// </summary>
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Type { get; set; } = "MultipleChoice";
        public List<string> Options { get; set; } = new();
        public string CorrectAnswer { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public int QuestionOrder { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Legacy properties for backward compatibility
        public int QuestionId { get => Id; set => Id = value; }
        public string OptionA { get => Options.Count > 0 ? Options[0] : string.Empty; set { if (Options.Count > 0) Options[0] = value; else Options.Add(value); } }
        public string OptionB { get => Options.Count > 1 ? Options[1] : string.Empty; set { if (Options.Count > 1) Options[1] = value; else if (Options.Count == 1) Options.Add(value); } }
        public string OptionC { get => Options.Count > 2 ? Options[2] : string.Empty; set { if (Options.Count > 2) Options[2] = value; else if (Options.Count == 2) Options.Add(value); } }
        public string OptionD { get => Options.Count > 3 ? Options[3] : string.Empty; set { if (Options.Count > 3) Options[3] = value; else if (Options.Count == 3) Options.Add(value); } }
        
        // Additional UI-specific properties
        public string CorrectAnswerDisplay => $"Correct Answer: {CorrectAnswer}";
    }
}
