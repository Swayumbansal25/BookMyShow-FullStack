using BookMyShow.Application.Extensions;
using BookMyShow.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using BookMyShow.WebApi.Configuration;
using BookMyShow.WebApi.Middleware;
using Serilog;

SerilogConfiguration.CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // 1. Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000") // Added 3000 as a backup
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    // Configure Serilog
    SerilogConfiguration.ConfigureSerilog(builder);

    // 🔹 Register Clean Architecture layers
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    // 🔹 ASP.NET Core services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "BookMyShow API",
            Version = "v1",
            Description = "BookMyShow Clean Architecture API"
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });

    // Add CORS Policy to allow Vercel Frontend requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercelFrontend",
        policy =>
        {
            policy.WithOrigins("https://book-my-show-full-stack.vercel.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

    var app = builder.Build();
    app.UseCors("AllowVercelFrontend");

    // 2. Enable Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";

        options.GetLevel = (context, elapsed, ex) =>
            ex != null || context.Response.StatusCode >= 500
                ? Serilog.Events.LogEventLevel.Error
                : Serilog.Events.LogEventLevel.Information;
    });

    // 🔹 Custom middleware
    app.UseCorrelationId();

    // 🔹 Swagger (Always available for testing)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookMyShow API V1");
        c.RoutePrefix = string.Empty;
    });

    // 3. IMPORTANT: Middleware Pipeline Order
    app.UseHttpsRedirection();
    app.UseRouting();

    // CORS MUST be between UseRouting and MapControllers
    app.UseCors("AllowFrontend"); 

    app.UseAuthorization();
    app.MapControllers();

    Log.Information("Application started successfully");
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