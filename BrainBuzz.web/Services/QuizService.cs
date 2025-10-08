using BrainBuzz.web.Data;
using BrainBuzz.web.Models.DbTable;
using BrainBuzz.web.Models.ViewModels;
using BrainBuzz.web.Models.Requests;
using BrainBuzz.web.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BrainBuzz.web.Services
{
    /// <summary>
    /// Service for handling quiz-related operations
    /// </summary>
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext _context;

        public QuizService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all quizzes
        /// </summary>
        public async Task<List<QuizViewModel>> GetAllQuizzesAsync()
        {
            var quizzes = await _context.Quizzes
                .Where(q => !q.IsDeleted)
                .OrderBy(q => q.QuizName)
                .ToListAsync();

            var result = new List<QuizViewModel>();
            foreach (var quiz in quizzes)
            {
                result.Add(await MapToQuizViewModelAsync(quiz));
            }

            return result;
        }

        /// <summary>
        /// Gets only active quizzes for customers
        /// </summary>
        public async Task<List<QuizViewModel>> GetActiveQuizzesAsync()
        {
            var quizzes = await _context.Quizzes
                .Where(q => q.IsActive && !q.IsDeleted)
                .OrderBy(q => q.QuizName)
                .ToListAsync();

            var result = new List<QuizViewModel>();
            foreach (var quiz in quizzes)
            {
                result.Add(await MapToQuizViewModelAsync(quiz));
            }

            return result;
        }

        /// <summary>
        /// Gets a quiz by ID
        /// </summary>
        public async Task<QuizViewModel?> GetQuizByIdAsync(int quizId)
        {
            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.QuizId == quizId && !q.IsDeleted);

            return quiz != null ? MapToQuizViewModel(quiz) : null;
        }

        /// <summary>
        /// Gets questions for a specific quiz
        /// </summary>
        public async Task<List<QuestionViewModel>> GetQuizQuestionsAsync(int quizId)
        {
            var questions = await _context.Questions
                .Where(q => q.QuizId == quizId && !q.IsDeleted)
                .ToListAsync();

            return questions.Select(MapToQuestionViewModel).ToList();
        }

        /// <summary>
        /// Gets quiz results for a user
        /// </summary>
        public async Task<List<QuizResultViewModel>> GetUserQuizResultsAsync(string username)
        {
            var results = await _context.QuizResults
                .Include(r => r.Quizzes)
                .Where(r => r.Username == username)
                .OrderByDescending(r => r.CompletedAt)
                .ToListAsync();

            return results.Select(MapToQuizResultViewModel).ToList();
        }

        /// <summary>
        /// Saves a quiz result
        /// </summary>
        public async Task<bool> SaveQuizResultAsync(QuizResultViewModel result)
        {
            try
            {
                // Validate result data
                if (result.Score < 0 || result.Score > result.TotalQuestions)
                    throw new ArgumentException("Invalid score data");

                if (result.Percentage < 0 || result.Percentage > 100)
                    throw new ArgumentException("Invalid percentage data");

                if (result.TimeSpent < 0)
                    throw new ArgumentException("Invalid time spent data");

                var quizResult = new Models.DbTable.QuizResults
                {
                    QuizId = result.QuizId,
                    Username = result.Username,
                    Score = result.Score,
                    TotalQuestions = result.TotalQuestions,
                    Percentage = result.Percentage,
                    CompletedAt = result.CompletedAt,
                    TimeSpent = result.TimeSpent,
                    UserAnswers = result.UserAnswers
                };

                _context.QuizResults.Add(quizResult);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandlingService.LogError(null!, ex, "Error saving quiz result for user: {Username}", result.Username);
                return false;
            }
        }

        /// <summary>
        /// Gets all quiz results (for admin purposes)
        /// </summary>
        public async Task<List<QuizResultViewModel>> GetAllQuizResultsAsync()
        {
            var results = await _context.QuizResults
                .Include(r => r.Quizzes)
                .OrderByDescending(r => r.CompletedAt)
                .ToListAsync();

            return results.Select(MapToQuizResultViewModel).ToList();
        }

        /// <summary>
        /// Gets quiz statistics for a specific quiz
        /// </summary>
        public async Task<object> GetQuizStatisticsAsync(int quizId)
        {
            var results = await _context.QuizResults
                .Where(r => r.QuizId == quizId)
                .ToListAsync();

            if (!results.Any())
            {
                return new
                {
                    TotalAttempts = 0,
                    AverageScore = 0.0,
                    AverageTime = 0,
                    BestScore = 0,
                    CompletionRate = 0.0
                };
            }

            return new
            {
                TotalAttempts = results.Count,
                AverageScore = Math.Round(results.Average(r => r.Percentage), 2),
                AverageTime = (int)results.Average(r => r.TimeSpent),
                BestScore = results.Max(r => r.Percentage),
                CompletionRate = Math.Round((double)results.Count(r => r.Score > 0) / results.Count * 100, 2)
            };
        }

        /// <summary>
        /// Maps database entity to ViewModel
        /// </summary>
        private async Task<QuizViewModel> MapToQuizViewModelAsync(Quizzes quiz)
        {
            var questions = await _context.Questions
                .Where(q => q.QuizId == quiz.QuizId && !q.IsDeleted)
                .ToListAsync();

            return new QuizViewModel
            {
                Id = quiz.QuizId,
                Title = quiz.QuizName,
                Description = quiz.Description,
                Category = quiz.Category,
                TimeLimitMinutes = quiz.TimeLimit > 0 ? quiz.TimeLimit : null,
                IsActive = quiz.IsActive,
                CreatedAt = quiz.CreatedDate,
                UpdatedAt = null, // Can be added later if needed
                Questions = questions.Select(MapToQuestionViewModel).ToList(),
                // Legacy properties
                QuizId = quiz.QuizId,
                QuizName = quiz.QuizName,
                TotalQuestions = quiz.TotalQuestions,
                TimeLimit = quiz.TimeLimit,
                Difficulty = quiz.Difficulty ?? "",
                CreatedDate = quiz.CreatedDate,
                UpdatedDate = null
            };
        }

        /// <summary>
        /// Maps database entity to ViewModel (synchronous version for backward compatibility)
        /// </summary>
        private static QuizViewModel MapToQuizViewModel(Quizzes quiz)
        {
            return new QuizViewModel
            {
                Id = quiz.QuizId,
                Title = quiz.QuizName,
                Description = quiz.Description,
                Category = quiz.Category,
                TimeLimitMinutes = quiz.TimeLimit > 0 ? quiz.TimeLimit : null,
                IsActive = quiz.IsActive,
                CreatedAt = quiz.CreatedDate,
                UpdatedAt = null,
                Questions = new List<QuestionViewModel>(), // Will be loaded separately
                // Legacy properties
                QuizId = quiz.QuizId,
                QuizName = quiz.QuizName,
                TotalQuestions = quiz.TotalQuestions,
                TimeLimit = quiz.TimeLimit,
                Difficulty = quiz.Difficulty ?? "",
                CreatedDate = quiz.CreatedDate,
                UpdatedDate = null
            };
        }

        /// <summary>
        /// Maps database entity to ViewModel
        /// </summary>
        private static QuestionViewModel MapToQuestionViewModel(Questions question)
        {
            var options = new List<string>();
            if (!string.IsNullOrEmpty(question.OptionA)) options.Add(question.OptionA);
            if (!string.IsNullOrEmpty(question.OptionB)) options.Add(question.OptionB);
            if (!string.IsNullOrEmpty(question.OptionC)) options.Add(question.OptionC);
            if (!string.IsNullOrEmpty(question.OptionD)) options.Add(question.OptionD);

            return new QuestionViewModel
            {
                Id = question.QuestionId,
                QuizId = question.QuizId,
                QuestionText = question.QuestionText,
                Type = "MultipleChoice", // Default type
                Options = options,
                CorrectAnswer = question.CorrectOption, // Store as option letter (A, B, C, D)
                Explanation = "",
                QuestionOrder = question.QuestionId,
                IsActive = true,
                // Legacy properties
                QuestionId = question.QuestionId,
                OptionA = question.OptionA,
                OptionB = question.OptionB,
                OptionC = question.OptionC,
                OptionD = question.OptionD
            };
        }

        /// <summary>
        /// Maps database entity to ViewModel
        /// </summary>
        private static QuizResultViewModel MapToQuizResultViewModel(Models.DbTable.QuizResults result)
        {
            return new QuizResultViewModel
            {
                ResultId = result.ResultId,
                QuizId = result.QuizId,
                QuizName = result.Quizzes?.QuizName ?? "Unknown Quiz",
                Username = result.Username,
                Score = result.Score,
                TotalQuestions = result.TotalQuestions,
                Percentage = result.Percentage,
                CompletedAt = result.CompletedAt,
                TimeSpent = result.TimeSpent,
                UserAnswers = result.UserAnswers
            };
        }

        /// <summary>
        /// Creates a new quiz
        /// </summary>
        public async Task<QuizViewModel> CreateQuizAsync(QuizRequest quizRequest)
        {
            var dbQuiz = new Quizzes
            {
                QuizName = quizRequest.Title,
                Description = quizRequest.Description,
                Category = quizRequest.Category,
                TimeLimit = quizRequest.TimeLimitMinutes ?? 0,
                IsActive = quizRequest.IsActive,
                CreatedDate = DateTime.UtcNow,
                TotalQuestions = quizRequest.Questions.Count
            };

            _context.Quizzes.Add(dbQuiz);
            await _context.SaveChangesAsync();

            // Add questions
            foreach (var question in quizRequest.Questions)
            {
                var dbQuestion = new Questions
                {
                    QuizId = dbQuiz.QuizId,
                    QuestionText = question.QuestionText,
                    OptionA = question.Options.Count > 0 ? question.Options[0] : "",
                    OptionB = question.Options.Count > 1 ? question.Options[1] : "",
                    OptionC = question.Options.Count > 2 ? question.Options[2] : "",
                    OptionD = question.Options.Count > 3 ? question.Options[3] : "",
                    CorrectOption = question.CorrectAnswer
                };

                _context.Questions.Add(dbQuestion);
            }

            await _context.SaveChangesAsync();

            return MapToQuizViewModel(dbQuiz);
        }

        /// <summary>
        /// Updates an existing quiz
        /// </summary>
        public async Task<QuizViewModel> UpdateQuizAsync(QuizRequest quizRequest)
        {
            var dbQuiz = await _context.Quizzes.FindAsync(quizRequest.Id);
            if (dbQuiz == null)
                throw new ArgumentException("Quiz not found");

            dbQuiz.QuizName = quizRequest.Title;
            dbQuiz.Description = quizRequest.Description;
            dbQuiz.Category = quizRequest.Category;
            dbQuiz.TimeLimit = quizRequest.TimeLimitMinutes ?? 0;
            dbQuiz.IsActive = quizRequest.IsActive;
            dbQuiz.TotalQuestions = quizRequest.Questions.Count;

            // Remove existing questions
            var existingQuestions = await _context.Questions
                .Where(q => q.QuizId == quizRequest.Id)
                .ToListAsync();
            _context.Questions.RemoveRange(existingQuestions);

            // Add updated questions
            foreach (var question in quizRequest.Questions)
            {
                var dbQuestion = new Questions
                {
                    QuizId = dbQuiz.QuizId,
                    QuestionText = question.QuestionText,
                    OptionA = question.Options.Count > 0 ? question.Options[0] : "",
                    OptionB = question.Options.Count > 1 ? question.Options[1] : "",
                    OptionC = question.Options.Count > 2 ? question.Options[2] : "",
                    OptionD = question.Options.Count > 3 ? question.Options[3] : "",
                    CorrectOption = question.CorrectAnswer
                };

                _context.Questions.Add(dbQuestion);
            }

            await _context.SaveChangesAsync();

            return MapToQuizViewModel(dbQuiz);
        }

        /// <summary>
        /// Deletes a quiz
        /// </summary>
        public async Task<bool> DeleteQuizAsync(int quizId)
        {
            var dbQuiz = await _context.Quizzes.FindAsync(quizId);
            if (dbQuiz == null || dbQuiz.IsDeleted)
                return false;

            // Soft delete related questions
            var questions = await _context.Questions
                .Where(q => q.QuizId == quizId && !q.IsDeleted)
                .ToListAsync();
            
            foreach (var question in questions)
            {
                question.IsDeleted = true;
            }

            // Soft delete the quiz
            dbQuiz.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
