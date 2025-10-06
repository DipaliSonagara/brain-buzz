using System.ComponentModel;

namespace BrainBuzz.web.Services
{
    /// <summary>
    /// Service for managing loading states across the application
    /// </summary>
    public interface ILoadingService : INotifyPropertyChanged
    {
        /// <summary>
        /// Indicates if any loading operation is in progress
        /// </summary>
        bool IsLoading { get; }
        
        /// <summary>
        /// Current loading message
        /// </summary>
        string LoadingMessage { get; }
        
        /// <summary>
        /// Current loading percentage (0-100)
        /// </summary>
        int LoadingPercentage { get; }
        
        /// <summary>
        /// Start a loading operation
        /// </summary>
        /// <param name="message">Loading message to display</param>
        /// <param name="percentage">Initial percentage (optional)</param>
        void StartLoading(string message = "Loading...", int percentage = 0);
        
        /// <summary>
        /// Update loading progress
        /// </summary>
        /// <param name="message">Updated message</param>
        /// <param name="percentage">Updated percentage</param>
        void UpdateLoading(string message, int percentage);
        
        /// <summary>
        /// Stop loading operation
        /// </summary>
        void StopLoading();
        
        /// <summary>
        /// Event fired when loading state changes
        /// </summary>
        event Action<bool, string>? LoadingStateChanged;
    }
}
