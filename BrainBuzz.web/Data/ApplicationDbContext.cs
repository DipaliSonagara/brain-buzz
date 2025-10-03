using BrainBuzz.web.Models.DbTable;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BrainBuzz.web.Data
{
    public class ApplicationDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public DbSet<Users> Users { get; set; }
        public DbSet<Quizzes> Quizzes { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<QuizResults> QuizResults { get; set; }
    }
}
