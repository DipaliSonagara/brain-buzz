using BrainBuzz.web.Models.DbTable;

namespace BrainBuzz.web.Services
{
    public class SessionService
    {
        private static readonly Dictionary<string, string> _activeSessions = new();
        private static readonly object _lock = new object();

        public void CreateSession(string sessionId, string username)
        {
            lock (_lock)
            {
                _activeSessions[sessionId] = username;
            }
        }

        public void RemoveSession(string sessionId)
        {
            lock (_lock)
            {
                _activeSessions.Remove(sessionId);
            }
        }

        public string? GetUserFromSession(string sessionId)
        {
            lock (_lock)
            {
                return _activeSessions.TryGetValue(sessionId, out var username) ? username : null;
            }
        }

        public bool IsUserLoggedIn(string sessionId)
        {
            lock (_lock)
            {
                var result = _activeSessions.ContainsKey(sessionId);
                return result;
            }
        }

        public string? GetUsername(string sessionId)
        {
            lock (_lock)
            {
                return _activeSessions.TryGetValue(sessionId, out var username) ? username : null;
            }
        }
    }
}
