using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBuzz.web.Models.DbTable
{
    public class QuizResults
    {
        [Key]
        public int ResultId { get; set; }
        
        [ForeignKey("Quizzes")]
        public int QuizId { get; set; }
        
        public string Username { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }
        public DateTime CompletedAt { get; set; }
        public int TimeSpent { get; set; } // in seconds
        public string UserAnswers { get; set; } = string.Empty; // JSON string of user answers
        
        // Navigation properties
        public Quizzes Quizzes { get; set; } = null!;
    }
}
