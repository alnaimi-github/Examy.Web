using Examy.Api.Data.Entities;
using Examy.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Examy.Api.Data
{
    public class QuizContext : DbContext
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        public QuizContext(DbContextOptions<QuizContext> options, IPasswordHasher<User> passwordHasher) : base(options)
        {
            _passwordHasher = passwordHasher;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<StudentQuiz> StudentQuizzes { get; set; }

        public DbSet<Option> Options { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<StudentQuizQuestion> StudentQuestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentQuizQuestion>().HasKey(s=> new
            {
                s.StudentQuizId, s.QuestionId
            });
            modelBuilder.Entity<StudentQuizQuestion>()
                .HasOne(s => s.StudentQuiz)
                .WithMany(s => s.StudentQuizQuestion)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<StudentQuizQuestion>()
                .HasOne(s => s.Question)
                .WithMany(s => s.StudentQuizQuestion)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);

            var adminUser = new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@gmail.com",
                Phone = "1234567890",
                Role = nameof(UserRole.Admin),
                IsApproved = true
            };
            var studentUser = new User
            {
                Id = 2,
                Name = "Student",
                Email = "student@gmail.com",
                Phone = "1234567890",
                Role = nameof(UserRole.Student),
                IsApproved = true

            };
            adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "Admin@123");
            studentUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "Student@123");
            
            modelBuilder.Entity<User>().HasData(adminUser);
            modelBuilder.Entity<User>().HasData(studentUser);
        }

    }
    
}
