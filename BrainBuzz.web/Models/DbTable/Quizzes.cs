using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace BrainBuzz.web.Models.DbTable

{
    public class Quizzes
    {
        [Key]
        public int QuizId { get; set; }
        public required string QuizName { get; set; }
        public required string Description { get; set; }
        public string Category { get; set; } = "General";
        public string Difficulty { get; set; } = "Medium";
        public int TimeLimit { get; set; }
        public int TotalQuestions { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Questions> Questions { get; set; } = new List<Questions>();
    }

    
}
