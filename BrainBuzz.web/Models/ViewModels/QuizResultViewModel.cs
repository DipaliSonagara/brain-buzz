namespace BrainBuzz.web.Models.ViewModels
{
    public class QuizResultViewModel
    {
        public int ResultId { get; set; }
        public int QuizId { get; set; }
        public string QuizName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }
        public DateTime CompletedAt { get; set; }
        public int TimeSpent { get; set; } // in seconds
        public string UserAnswers { get; set; } = string.Empty;
        
        // UI-specific properties
        public string TimeSpentDisplay => FormatTime(TimeSpent);
        public string CompletedAtDisplay => CompletedAt.ToString("MMM dd, yyyy HH:mm");
        public string Grade => GetGrade(Percentage);
        public string GradeColor => GetGradeColor(Percentage);
        
        private static string FormatTime(int seconds)
        {
            var minutes = seconds / 60;
            var remainingSeconds = seconds % 60;
            return $"{minutes}:{remainingSeconds:D2}";
        }
        
        private static string GetGrade(double percentage)
        {
            return percentage switch
            {
                >= 90 => "A+",
                >= 80 => "A",
                >= 70 => "B",
                >= 60 => "C",
                >= 50 => "D",
                _ => "F"
            };
        }
        
        private static string GetGradeColor(double percentage)
        {
            return percentage switch
            {
                >= 80 => "text-success",
                >= 60 => "text-warning",
                _ => "text-danger"
            };
        }
    }
}