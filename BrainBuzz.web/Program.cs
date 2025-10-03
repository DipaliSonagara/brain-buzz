using BrainBuzz.web.Components;
using BrainBuzz.web.Data;
using BrainBuzz.web.Services;
using BrainBuzz.web.Services.Interface;
using BrainBuzz.web.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;

// Configure Serilog


try
{
    Log.Information("Starting BrainBuzz application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    // Configure database settings
    builder.Services.Configure<DatabaseSettings>(
        builder.Configuration.GetSection(DatabaseSettings.SectionName));
    
    // Configure security settings
    builder.Services.Configure<SecuritySettings>(
        builder.Configuration.GetSection(SecuritySettings.SectionName));

    var databaseSettings = builder.Configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>() ?? new DatabaseSettings();
    var securitySettings = builder.Configuration.GetSection(SecuritySettings.SectionName).Get<SecuritySettings>() ?? new SecuritySettings();

    // Configure Entity Framework
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection") ?? 
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
            sqlOptions =>
            {
                sqlOptions.CommandTimeout(databaseSettings.CommandTimeout);
            });
        
        if (databaseSettings.EnableSensitiveDataLogging)
        {
            options.EnableSensitiveDataLogging();
        }
    });

    // Configure Identity with proper security settings
    builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    {
        var passwordReq = securitySettings.PasswordRequirements;
        options.Password.RequireDigit = passwordReq.RequireDigit;
        options.Password.RequireLowercase = passwordReq.RequireLowercase;
        options.Password.RequireNonAlphanumeric = passwordReq.RequireNonAlphanumeric;
        options.Password.RequireUppercase = passwordReq.RequireUppercase;
        options.Password.RequiredLength = passwordReq.MinimumLength;
        options.Password.RequiredUniqueChars = passwordReq.RequiredUniqueChars;
        
        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        
        // User settings
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
        
        // Sign-in settings
        options.SignIn.RequireConfirmedEmail = false; // Set to true for production
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

    // Register application services
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IQuizService, QuizService>();
    builder.Services.AddScoped<IEnhancedAuthenticationService, EnhancedAuthenticationService>();
    builder.Services.AddSingleton<SessionService>();
    builder.Services.AddScoped<AuthenticationService>(); // Keep for backward compatibility

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    // Security enhancements
    if (securitySettings.RequireHttps)
    {
        app.UseHttpsRedirection();
    }

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    // Database initialization
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Log.Information("Initializing database...");
            DataSeeder.Seed(dbContext);
            Log.Information("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    Log.Information("BrainBuzz application started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}