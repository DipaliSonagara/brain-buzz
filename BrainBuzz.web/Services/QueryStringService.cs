using BrainBuzz.web.Services.Interface;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Components;

namespace BrainBuzz.web.Services
{
    public class QueryStringService : IQueryStringService
    {
        private readonly NavigationManager _navigationManager;

        public QueryStringService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public Task<string> GetQueryStringAsync(string key)
        {
            var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
            var queryParams = QueryHelpers.ParseQuery(uri.Query);
            
            if (queryParams.TryGetValue(key, out var value))
            {
                return Task.FromResult(value.ToString());
            }
            
            return Task.FromResult(string.Empty);
        }
    }
}
