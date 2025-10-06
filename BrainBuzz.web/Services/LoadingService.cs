using System.ComponentModel;

namespace BrainBuzz.web.Services
{
    /// <summary>
    /// Service for managing loading states across the application
    /// </summary>
    public class LoadingService : ILoadingService, INotifyPropertyChanged
    {
        private bool _isLoading = false;
        private string _loadingMessage = "Loading...";
        private int _loadingPercentage = 0;
        private readonly object _lock = new object();

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                    LoadingStateChanged?.Invoke(_isLoading, _loadingMessage);
                }
            }
        }

        public string LoadingMessage
        {
            get => _loadingMessage;
            private set
            {
                if (_loadingMessage != value)
                {
                    _loadingMessage = value;
                    OnPropertyChanged(nameof(LoadingMessage));
                }
            }
        }

        public int LoadingPercentage
        {
            get => _loadingPercentage;
            private set
            {
                if (_loadingPercentage != value)
                {
                    _loadingPercentage = value;
                    OnPropertyChanged(nameof(LoadingPercentage));
                }
            }
        }

        public event Action<bool, string>? LoadingStateChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void StartLoading(string message = "Loading...", int percentage = 0)
        {
            lock (_lock)
            {
                LoadingMessage = message;
                LoadingPercentage = percentage;
                IsLoading = true;
            }
        }

        public void UpdateLoading(string message, int percentage)
        {
            lock (_lock)
            {
                if (_isLoading)
                {
                    LoadingMessage = message;
                    LoadingPercentage = Math.Clamp(percentage, 0, 100);
                }
            }
        }

        public void StopLoading()
        {
            lock (_lock)
            {
                IsLoading = false;
                LoadingMessage = "Loading...";
                LoadingPercentage = 0;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
