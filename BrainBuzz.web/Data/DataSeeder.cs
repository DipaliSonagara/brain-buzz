using BrainBuzz.web.Models.DbTable;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace BrainBuzz.web.Data;

public static class DataSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        context.Database.Migrate(); // Apply migrations

        if (!context.Quizzes.Any())
        {
            var quizzes = new List<Quizzes>
            {
                new Quizzes { QuizId = 1, QuizName = "HTML-CSS", Description = "Test your knowledge of building and styling web pages with HTML & CSS!", TimeLimit = 10, TotalQuestions = 3 },
                new Quizzes { QuizId = 2, QuizName = "SQL", Description = "Test your database knowledge with SQL queries and concepts.", TimeLimit = 10, TotalQuestions = 3 },
                new Quizzes { QuizId = 3, QuizName = "Python", Description = "Check your Python programming skills — logic, syntax, and more!", TimeLimit = 10, TotalQuestions = 3 }
            };

            context.Quizzes.AddRange(quizzes);
            context.SaveChanges();

            var questions = new List<Questions>
            {
                // HTML-CSS Questions
                new Questions { QuizId = quizzes[0].QuizId, QuestionText = "What does CSS stand for?", OptionA = "Colorful Style Sheets", OptionB = "Cascading Style Sheets", OptionC = "Computer Style Sheets", OptionD = "Creative Style System", CorrectOption = "B" },
                new Questions { QuizId = quizzes[0].QuizId, QuestionText = "Which HTML tag is used to define an internal style sheet?", OptionA = "<css>", OptionB = "<script>", OptionC = "<style>", OptionD = "<link>", CorrectOption = "C" },
                new Questions { QuizId = quizzes[0].QuizId, QuestionText = "Which property changes text color in CSS?", OptionA = "font-color", OptionB = "color", OptionC = "text-color", OptionD = "background-color", CorrectOption = "B" },

                // SQL Questions
                new Questions { QuizId = quizzes[1].QuizId, QuestionText = "Which SQL keyword is used to extract data from a database?", OptionA = "OPEN", OptionB = "GET", OptionC = "EXTRACT", OptionD = "SELECT", CorrectOption = "D" },
                new Questions { QuizId = quizzes[1].QuizId, QuestionText = "What does DML stand for?", OptionA = "Data Manipulation Language", OptionB = "Database Management Language", OptionC = "Data Management Logic", OptionD = "Dynamic Markup Language", CorrectOption = "A" },
                new Questions { QuizId = quizzes[1].QuizId, QuestionText = "Which SQL clause is used to filter records?", OptionA = "ORDER BY", OptionB = "FILTER", OptionC = "WHERE", OptionD = "HAVING", CorrectOption = "C" },

                // Python Questions
                new Questions { QuizId = quizzes[2].QuizId, QuestionText = "Which symbol is used for comments in Python?", OptionA = "//", OptionB = "/* */", OptionC = "#", OptionD = "<!-- -->", CorrectOption = "C" },
                new Questions { QuizId = quizzes[2].QuizId, QuestionText = "Which keyword is used to define a function in Python?", OptionA = "func", OptionB = "define", OptionC = "def", OptionD = "function", CorrectOption = "C" },
                new Questions { QuizId = quizzes[2].QuizId, QuestionText = "What is the output of: len('BrainBuzz')?", OptionA = "7", OptionB = "8", OptionC = "9", OptionD = "10", CorrectOption = "C" }
            };

            context.Questions.AddRange(questions);
            context.SaveChanges();
        }
    }
}


