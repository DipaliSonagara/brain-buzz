namespace BrainBuzz.web.Services
{
    /// <summary>
    /// Service for managing toast notifications
    /// </summary>
    public class ToastService
    {
        /// <summary>
        /// Event triggered when a toast should be shown
        /// </summary>
        public event Action<ToastMessage>? OnShowToast;

        /// <summary>
        /// Shows a success toast
        /// </summary>
        public void ShowSuccess(string message, string? title = null)
        {
            ShowToast(ToastType.Success, message, title);
        }

        /// <summary>
        /// Shows an error toast
        /// </summary>
        public void ShowError(string message, string? title = null)
        {
            ShowToast(ToastType.Error, message, title);
        }

        /// <summary>
        /// Shows a warning toast
        /// </summary>
        public void ShowWarning(string message, string? title = null)
        {
            ShowToast(ToastType.Warning, message, title);
        }

        /// <summary>
        /// Shows an info toast
        /// </summary>
        public void ShowInfo(string message, string? title = null)
        {
            ShowToast(ToastType.Info, message, title);
        }

        /// <summary>
        /// Shows a toast with custom type
        /// </summary>
        public void ShowToast(ToastType type, string message, string? title = null)
        {
            var toast = new ToastMessage
            {
                Id = Guid.NewGuid().ToString(),
                Type = type,
                Title = title,
                Message = message,
                Timestamp = DateTime.Now
            };

            OnShowToast?.Invoke(toast);
        }
    }

    /// <summary>
    /// Toast message model
    /// </summary>
    public class ToastMessage
    {
        public string Id { get; set; } = string.Empty;
        public ToastType Type { get; set; }
        public string? Title { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsVisible { get; set; } = true;
    }

    /// <summary>
    /// Toast types
    /// </summary>
    public enum ToastType
    {
        Success,
        Error,
        Warning,
        Info
    }
}

