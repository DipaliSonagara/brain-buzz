using BrainBuzz.web.Data;
using BrainBuzz.web.Models.DbTable;
using BrainBuzz.web.Models.Request;
using BrainBuzz.web.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BrainBuzz.web.Services
{
     public class UserService : IUserService
     {
          private readonly ApplicationDbContext applicationDb;
          private readonly UserManager<IdentityUser> userManager;
          private readonly SignInManager<IdentityUser> signInManager;
          private readonly SessionService sessionService;

          public UserService(ApplicationDbContext applicationDb, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, SessionService sessionService)
          {
               this.applicationDb = applicationDb;
               this.userManager = userManager;
               this.signInManager = signInManager;
               this.sessionService = sessionService;
          }

          public async Task AddUser(RegisterRequest registerRequest)
          {
               var user = new IdentityUser 
               { 
                   UserName = registerRequest.Username, 
                   Email = registerRequest.Email 
               };
               
               var result = await userManager.CreateAsync(user, registerRequest.Password);
               
               if (!result.Succeeded)
               {
                   throw new Exception($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
               }
               
               var legacyUser = new Users()
               {
                 Email = registerRequest.Email,
                 Username = registerRequest.Username,
                 PasswordHash = "HASHED_BY_IDENTITY" 
               };
               await applicationDb.Users.AddAsync(legacyUser);
               await applicationDb.SaveChangesAsync();
          }

          public async Task<bool> LoginUser(LoginRequest loginRequest)
          {
               var user = await userManager.FindByNameAsync(loginRequest.Username);
               if (user == null)
                    return false;
               
               // Verify password without signing in (to avoid cookie issues in Blazor Server)
               var isValidPassword = await userManager.CheckPasswordAsync(user, loginRequest.Password);
               
               if (isValidPassword)
               {
                    // Create a simple session (in a real app, you'd use proper session management)
                    var sessionId = Guid.NewGuid().ToString();
                    sessionService.CreateSession(sessionId, loginRequest.Username);
               }
               
               return isValidPassword;
          }
     }
}
