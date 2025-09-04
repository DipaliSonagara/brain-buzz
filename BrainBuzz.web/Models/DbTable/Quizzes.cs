using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BrainBuzz.web.Models.DbTable

{
    public class Quizzes
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public string Description { get; set; }
        public int TimeLimit { get; set; }
        public int TotalQuestions { get; set; }

        public ICollection<Questions> Questions { get; set; }
    }

    
}
