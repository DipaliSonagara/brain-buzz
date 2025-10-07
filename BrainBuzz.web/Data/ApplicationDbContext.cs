using BrainBuzz.web.Models.DbTable;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BrainBuzz.web.Data
{
    public class ApplicationDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public DbSet<Quizzes> Quizzes { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<QuizResults> QuizResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Quizzes entity
            modelBuilder.Entity<Quizzes>(entity =>
            {
                entity.HasKey(e => e.QuizId);
                entity.Property(e => e.QuizName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.Difficulty).HasMaxLength(50);
                
                // Indexes for performance
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Difficulty);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.CreatedDate);
            });

            // Configure Questions entity
            modelBuilder.Entity<Questions>(entity =>
            {
                entity.HasKey(e => e.QuestionId);
                entity.Property(e => e.QuestionText).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.OptionA).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OptionB).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OptionC).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OptionD).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CorrectOption).IsRequired().HasMaxLength(1);
                
                // Foreign key relationship
                entity.HasOne(e => e.Quizzes)
                      .WithMany(q => q.Questions)
                      .HasForeignKey(e => e.QuizId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Index for performance
                entity.HasIndex(e => e.QuizId);
            });

            // Configure QuizResults entity
            modelBuilder.Entity<QuizResults>(entity =>
            {
                entity.HasKey(e => e.ResultId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(256);
                entity.Property(e => e.UserAnswers).HasMaxLength(4000);
                
                // Foreign key relationship
                entity.HasOne(e => e.Quizzes)
                      .WithMany()
                      .HasForeignKey(e => e.QuizId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Indexes for performance
                entity.HasIndex(e => e.Username);
                entity.HasIndex(e => e.QuizId);
                entity.HasIndex(e => e.CompletedAt);
                entity.HasIndex(e => e.Percentage);
            });
        }
    }
}
