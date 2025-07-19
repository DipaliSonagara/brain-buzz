using BrainBuzz.web.Data;
using BrainBuzz.web.Models.DbTable;
using BrainBuzz.web.Models.Request;
using BrainBuzz.web.Services.Interface;

namespace BrainBuzz.web.Services
{
     public class UserService : IUserService
     {
          private readonly ApplicationDbContext applicationDb;

          public UserService(ApplicationDbContext applicationDb)
          {
               this.applicationDb = applicationDb;
          }

          public async Task AddUser(RegisterRequest registerRequest)
          {
               Users user = new Users()
               {
                 Email = registerRequest.Email,
                 FirstName = registerRequest.FirstName,
                 LastName = registerRequest.LastName,
                 PasswordHash = registerRequest.Password,
                 Username = registerRequest.Username,
               };
               await applicationDb.Users.AddAsync(user);
               await applicationDb.SaveChangesAsync();
          }
     }
}
