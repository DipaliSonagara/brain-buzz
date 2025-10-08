using BrainBuzz.web.Models.ViewModels;

namespace BrainBuzz.web.Services.Interface
{
    /// <summary>
    /// Interface for Quiz-related operations
    /// </summary>
    public interface IQuizService
    {
        /// <summary>
        /// Gets all active quizzes
        /// </summary>
        /// <returns>List of quiz view models</returns>
        Task<List<QuizViewModel>> GetAllQuizzesAsync();
        
        /// <summary>
        /// Gets a quiz by ID
        /// </summary>
        /// <param name="quizId">The quiz ID</param>
        /// <returns>Quiz view model or null if not found</returns>
        Task<QuizViewModel?> GetQuizByIdAsync(int quizId);
        
        /// <summary>
        /// Gets questions for a specific quiz
        /// </summary>
        /// <param name="quizId">The quiz ID</param>
        /// <returns>List of question view models</returns>
        Task<List<QuestionViewModel>> GetQuizQuestionsAsync(int quizId);
        
        /// <summary>
        /// Gets quiz results for a user
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>List of quiz result view models</returns>
        Task<List<QuizResultViewModel>> GetUserQuizResultsAsync(string username);
        
        /// <summary>
        /// Saves a quiz result
        /// </summary>
        /// <param name="result">The quiz result to save</param>
        /// <returns>True if saved successfully</returns>
        Task<bool> SaveQuizResultAsync(QuizResultViewModel result);
        
        /// <summary>
        /// Gets all quiz results (for admin purposes)
        /// </summary>
        /// <returns>List of all quiz result view models</returns>
        Task<List<QuizResultViewModel>> GetAllQuizResultsAsync();
        
        /// <summary>
        /// Gets quiz statistics for a specific quiz
        /// </summary>
        /// <param name="quizId">The quiz ID</param>
        /// <returns>Quiz statistics object</returns>
        Task<object> GetQuizStatisticsAsync(int quizId);
    }
}