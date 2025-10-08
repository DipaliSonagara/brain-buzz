using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBuzz.web.Models.DbTable
{
    public class Questions
    {
        [Key]
        public int QuestionId { get; set; }

        [ForeignKey("Quizzes")]
        public int QuizId { get; set; }
        public required string QuestionText { get; set; }
        public required string OptionA { get; set; }
        public required string OptionB { get; set; }
        public required string OptionC { get; set; }
        public required string OptionD { get; set; }
        public required string CorrectOption { get; set; }
        public bool IsDeleted { get; set; } = false;

        
        public Quizzes? Quizzes { get; set; }
    }
}

