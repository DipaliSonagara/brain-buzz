namespace BrainBuzz.web.Services.Interface
{
    public interface IQueryStringService
    {
        Task<string> GetQueryStringAsync(string key);
    }
}
