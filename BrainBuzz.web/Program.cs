using BrainBuzz.web.Components;
using BrainBuzz.web.Data;
using BrainBuzz.web.Services;
using BrainBuzz.web.Services.Interface;
using BrainBuzz.web.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Serilog;

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

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                              string.Empty;
        
        options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.CommandTimeout(databaseSettings.CommandTimeout);
        });
        
        if (databaseSettings.EnableSensitiveDataLogging)
        {
            options.EnableSensitiveDataLogging();
        }
    });

    builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;
        
        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        
        // User settings
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false; // Simplified for development
        
        // Sign-in settings
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

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
        app.UseHsts();
    }


    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.UseSession();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Log.Information("Initializing database...");
            
            dbContext.Database.EnsureCreated();
            
            DataSeeder.Seed(dbContext);
            Log.Information("Database initialized successfully");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while initializing the database. Application will continue without database seeding.");
    }

    Log.Information("BrainBuzz application started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}