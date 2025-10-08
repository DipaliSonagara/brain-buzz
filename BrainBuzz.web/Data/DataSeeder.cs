using BrainBuzz.web.Models.DbTable;
using BrainBuzz.web.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;

namespace BrainBuzz.web.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext context, 
        RoleManager<IdentityRole> roleManager, 
        UserManager<IdentityUser> userManager,
        IConfiguration configuration)
    {
        try
        {
            await context.Database.MigrateAsync();
            await SeedRolesAsync(roleManager);
            await SeedAdminUserAsync(userManager, configuration);
            await SeedQuizzesAsync(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during database seeding");
            throw;
        }
    }
    
    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var rolesList = string.Join(", ", AppRoles.AllRoles);
        foreach (var roleName in AppRoles.AllRoles)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    Log.Information("Role '{RoleName}' created successfully", roleName);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            else
            {
                Log.Information("Role '{RoleName}' already exists", roleName);
            }
        }
    }
    
    private static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        var adminEmail = configuration["AdminUser:Email"] ?? "admin@brainbuzz.com";
        var adminUsername = configuration["AdminUser:Username"] ?? "admin";
        var adminPassword = configuration["AdminUser:Password"] ?? "Admin@123";
        
        if (string.IsNullOrEmpty(adminPassword) || adminPassword == "Admin@123")
        {
            Log.Warning("SECURITY WARNING: Using default admin password. Please set AdminUser:Password in environment variables or secrets!");
        }
        
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
                Log.Information("Admin user '{Username}' created successfully with email '{Email}'", adminUsername, adminEmail);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Log.Error("Failed to create admin user: {Errors}", errors);
            }
        }
        else
        {
            Log.Information("Admin user already exists with email '{Email}'", adminEmail);
            
            if (!await userManager.IsInRoleAsync(existingAdmin, AppRoles.Admin))
            {
                await userManager.AddToRoleAsync(existingAdmin, AppRoles.Admin);
                Log.Information("Admin role assigned to existing user '{Email}'", adminEmail);
            }
        }
    }
    
    private static async Task SeedQuizzesAsync(ApplicationDbContext context)
    {
        var existingQuizCount = await context.Quizzes.CountAsync();
        if (existingQuizCount == 0)
        {
            var quizzes = new List<Quizzes>
            {
                new Quizzes { 
                    QuizName = "HTML-CSS Basics", 
                    Description = "Test your knowledge of building and styling web pages with HTML & CSS!", 
                    Category = "Web Development",
                    Difficulty = "Easy",
                    TimeLimit = 10, 
                    TotalQuestions = 3
                },
                new Quizzes { 
                    QuizName = "SQL Fundamentals", 
                    Description = "Test your database knowledge with SQL queries and concepts.", 
                    Category = "Database",
                    Difficulty = "Medium",
                    TimeLimit = 15, 
                    TotalQuestions = 3,
                },
                new Quizzes { 
                    QuizName = "Python Programming", 
                    Description = "Check your Python programming skills — logic, syntax, and more!", 
                    Category = "Programming",
                    Difficulty = "Medium",
                    TimeLimit = 15, 
                    TotalQuestions = 3,
                },
                new Quizzes { 
                    QuizName = "JavaScript Essentials", 
                    Description = "Master JavaScript fundamentals including variables, functions, and DOM manipulation.", 
                    Category = "Web Development",
                    Difficulty = "Hard",
                    TimeLimit = 20, 
                    TotalQuestions = 5,
                },
                new Quizzes { 
                    QuizName = "C# Basics", 
                    Description = "Test your knowledge of C# programming language fundamentals.", 
                    Category = "Programming",
                    Difficulty = "Medium",
                    TimeLimit = 15, 
                    TotalQuestions = 4,
                }
            };

            await context.Quizzes.AddRangeAsync(quizzes);
            await context.SaveChangesAsync();
            var questions = new List<Questions>
            {
                // HTML-CSS Questions
                new Questions { QuizId = quizzes[0].QuizId, QuestionText = "What does CSS stand for?", OptionA = "Colorful Style Sheets", OptionB = "Cascading Style Sheets", OptionC = "Computer Style Sheets", OptionD = "Creative Style System", CorrectOption = "B" },
                new Questions { QuizId = quizzes[0].QuizId, QuestionText = "Which HTML tag is used to define an internal style sheet?", OptionA = "<css>", OptionB = "<script>", OptionC = "<style>", OptionD = "<link>", CorrectOption = "C" },
                new Questions { QuizId = quizzes[0].QuizId, QuestionText = "Which property changes text color in CSS?", OptionA = "font-color", OptionB = "color", OptionC = "text-color", OptionD = "background-color", CorrectOption = "B" },

                // SQL Questions
                new Questions { QuizId = quizzes[1].QuizId, QuestionText = "Which SQL keyword is used to extract data from a database?", OptionA = "OPEN", OptionB = "GET", OptionC = "EXTRACT", OptionD = "SELECT", CorrectOption = "D" },
                new Questions { QuizId = quizzes[1].QuizId, QuestionText = "What does DML stand for?", OptionA = "Data Manipulation Language", OptionB = "Database Management Language", OptionC = "Data Management Logic", OptionD = "Dynamic Markup Language", CorrectOption = "A" },
                new Questions { QuizId = quizzes[1].QuizId, QuestionText = "Which SQL clause is used to filter records?", OptionA = "ORDER BY", OptionB = "FILTER", OptionC = "WHERE", OptionD = "HAVING", CorrectOption = "C" },

                // Python Questions
                new Questions { QuizId = quizzes[2].QuizId, QuestionText = "Which symbol is used for comments in Python?", OptionA = "//", OptionB = "/* */", OptionC = "#", OptionD = "<!-- -->", CorrectOption = "C" },
                new Questions { QuizId = quizzes[2].QuizId, QuestionText = "Which keyword is used to define a function in Python?", OptionA = "func", OptionB = "define", OptionC = "def", OptionD = "function", CorrectOption = "C" },
                new Questions { QuizId = quizzes[2].QuizId, QuestionText = "What is the output of: len('BrainBuzz')?", OptionA = "7", OptionB = "8", OptionC = "9", OptionD = "10", CorrectOption = "C" },

                // JavaScript Questions
                new Questions { QuizId = quizzes[3].QuizId, QuestionText = "Which keyword is used to declare a variable in JavaScript?", OptionA = "var", OptionB = "let", OptionC = "const", OptionD = "All of the above", CorrectOption = "D" },
                new Questions { QuizId = quizzes[3].QuizId, QuestionText = "What is the result of: typeof null?", OptionA = "null", OptionB = "undefined", OptionC = "object", OptionD = "string", CorrectOption = "C" },
                new Questions { QuizId = quizzes[3].QuizId, QuestionText = "Which method is used to add an element to the end of an array?", OptionA = "push()", OptionB = "pop()", OptionC = "shift()", OptionD = "unshift()", CorrectOption = "A" },
                new Questions { QuizId = quizzes[3].QuizId, QuestionText = "What does DOM stand for?", OptionA = "Document Object Model", OptionB = "Data Object Management", OptionC = "Dynamic Object Method", OptionD = "Document Oriented Markup", CorrectOption = "A" },
                new Questions { QuizId = quizzes[3].QuizId, QuestionText = "Which operator is used for strict equality in JavaScript?", OptionA = "==", OptionB = "===", OptionC = "=", OptionD = "!=", CorrectOption = "B" },

                // C# Questions
                new Questions { QuizId = quizzes[4].QuizId, QuestionText = "Which keyword is used to create a class in C#?", OptionA = "class", OptionB = "Class", OptionC = "CLASS", OptionD = "object", CorrectOption = "A" },
                new Questions { QuizId = quizzes[4].QuizId, QuestionText = "What is the default access modifier for class members in C#?", OptionA = "public", OptionB = "private", OptionC = "protected", OptionD = "internal", CorrectOption = "B" },
                new Questions { QuizId = quizzes[4].QuizId, QuestionText = "Which keyword is used to inherit from a class in C#?", OptionA = "extends", OptionB = "inherits", OptionC = ":", OptionD = "implements", CorrectOption = "C" },
                new Questions { QuizId = quizzes[4].QuizId, QuestionText = "What is the correct way to declare a string variable in C#?", OptionA = "string name = \"Hello\";", OptionB = "String name = \"Hello\";", OptionC = "var name = \"Hello\";", OptionD = "All of the above", CorrectOption = "D" }
            };

            await context.Questions.AddRangeAsync(questions);
            await context.SaveChangesAsync();
        }
        else
        {
            Log.Information("Quizzes already exist in database, skipping seed");
        }
    }
}