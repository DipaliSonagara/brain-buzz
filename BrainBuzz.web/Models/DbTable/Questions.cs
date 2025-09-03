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
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectOption { get; set; }

        
        public Quizzes Quizzes { get; set; }
    }
}

