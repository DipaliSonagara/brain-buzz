using BrainBuzz.web.Models.Request;

namespace BrainBuzz.web.Services.Interface
{
     public interface IUserService
     {
          Task AddUser(RegisterRequest registerRequest);
     }
}
