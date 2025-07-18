using System.ComponentModel.DataAnnotations;

namespace BrainBuzz.web.Models.DbTable
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string? Role { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? Email { get; set; }
    }
}
