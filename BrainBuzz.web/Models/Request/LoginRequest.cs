using Azure.Identity;
using System.ComponentModel.DataAnnotations;


namespace BrainBuzz.web.Models.Request
{
     public class LoginRequest
     {
          [Required]
          public string Username { get; set; } = string.Empty;

          [Required]
          public string Password { get; set; } = string.Empty;

     }
}
