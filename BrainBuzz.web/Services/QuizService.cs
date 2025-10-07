using BrainBuzz.web.Data;
using BrainBuzz.web.Models.DbTable;
using BrainBuzz.web.Models.ViewModels;
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
        private readonly ILoadingService _loadingService;

        public QuizService(ApplicationDbContext context, ILoadingService loadingService)
        {
            _context = context;
            _loadingService = loadingService;
        }

        /// <summary>
        /// Gets all quizzes
        /// </summary>
        public async Task<List<QuizViewModel>> GetAllQuizzesAsync()
        {
            _loadingService.StartLoading("Loading quizzes...");
            
            try
            {
                var quizzes = await _context.Quizzes
                    .OrderBy(q => q.QuizName)
                    .ToListAsync();

                _loadingService.UpdateLoading("Processing quiz data...", 50);
                var result = quizzes.Select(MapToQuizViewModel).ToList();
                
                _loadingService.StopLoading();
                return result;
            }
            catch (Exception)
            {
                _loadingService.StopLoading();
                throw;
            }
        }

        /// <summary>
        /// Gets a quiz by ID
        /// </summary>
        public async Task<QuizViewModel?> GetQuizByIdAsync(int quizId)
        {
            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.QuizId == quizId);

            return quiz != null ? MapToQuizViewModel(quiz) : null;
        }

        /// <summary>
        /// Gets questions for a specific quiz
        /// </summary>
        public async Task<List<QuestionViewModel>> GetQuizQuestionsAsync(int quizId)
        {
            _loadingService.StartLoading("Loading quiz questions...");
            
            try
            {
                var questions = await _context.Questions
                    .Where(q => q.QuizId == quizId)
                    .ToListAsync();

                _loadingService.UpdateLoading("Preparing questions...", 75);
                var result = questions.Select(MapToQuestionViewModel).ToList();
                
                _loadingService.StopLoading();
                return result;
            }
            catch (Exception)
            {
                _loadingService.StopLoading();
                throw;
            }
        }

        /// <summary>
        /// Gets quiz results for a user
        /// </summary>
        public async Task<List<QuizResultViewModel>> GetUserQuizResultsAsync(string username)
        {
            _loadingService.StartLoading("Loading your quiz results...");
            
            try
            {
                var results = await _context.QuizResults
                    .Include(r => r.Quizzes)
                    .Where(r => r.Username == username)
                    .OrderByDescending(r => r.CompletedAt)
                    .ToListAsync();

                _loadingService.UpdateLoading("Processing results...", 60);
                var result = results.Select(MapToQuizResultViewModel).ToList();
                
                _loadingService.StopLoading();
                return result;
            }
            catch (Exception)
            {
                _loadingService.StopLoading();
                throw;
            }
        }

        /// <summary>
        /// Saves a quiz result
        /// </summary>
        public async Task<bool> SaveQuizResultAsync(QuizResultViewModel result)
        {
            _loadingService.StartLoading("Saving your quiz result...");
            
            try
            {
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

                _loadingService.UpdateLoading("Processing your score...", 50);
                _context.QuizResults.Add(quizResult);
                await _context.SaveChangesAsync();
                
                _loadingService.StopLoading();
                return true;
            }
            catch (Exception ex)
            {
                _loadingService.StopLoading();
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
        private static QuizViewModel MapToQuizViewModel(Quizzes quiz)
        {
            return new QuizViewModel
            {
                QuizId = quiz.QuizId,
                QuizName = quiz.QuizName,
                Description = quiz.Description,
                TotalQuestions = quiz.TotalQuestions,
                TimeLimit = quiz.TimeLimit,
                Category = quiz.Category,
                Difficulty = quiz.Difficulty,
                IsActive = quiz.IsActive,
                CreatedDate = quiz.CreatedDate,
                UpdatedDate = null // Can be added later if needed
            };
        }

        /// <summary>
        /// Maps database entity to ViewModel
        /// </summary>
        private static QuestionViewModel MapToQuestionViewModel(Questions question)
        {
            return new QuestionViewModel
            {
                QuestionId = question.QuestionId,
                QuizId = question.QuizId,
                QuestionText = question.QuestionText,
                OptionA = question.OptionA,
                OptionB = question.OptionB,
                OptionC = question.OptionC,
                OptionD = question.OptionD,
                CorrectAnswer = question.CorrectOption, // Map CorrectOption to CorrectAnswer
                Explanation = "", // Default empty since it doesn't exist in DB
                QuestionOrder = question.QuestionId, // Use QuestionId as order since QuestionOrder doesn't exist
                IsActive = true // Default to true since it doesn't exist in DB
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
    }
}
