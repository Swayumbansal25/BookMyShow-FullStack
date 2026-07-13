using BookMyShow.Application.Extensions;
using BookMyShow.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Net;
using System.Text.Json;
using BookMyShow.WebApi.Configuration;
using BookMyShow.WebApi.Middleware;
using Serilog;

SerilogConfiguration.CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // 1. Configure a single CORS policy (local dev + live Vercel origin)
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins(
                        "http://localhost:5173",
                        "http://localhost:3000",
                        "https://book-my-show-full-stack.vercel.app"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
    });

    // Configure Serilog
    SerilogConfiguration.ConfigureSerilog(builder);

    // Register Clean Architecture layers
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    // ASP.NET Core services
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

    var app = builder.Build();

    // 2. Global exception handler — MUST be one of the first things registered so
    //    that even unhandled exceptions produce a response with CORS headers attached,
    //    instead of a bare 500 that the browser misreports as a CORS failure.
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var exceptionHandlerPathFeature =
                context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            if (exception != null)
            {
                Log.Error(exception, "Unhandled exception on {Path}", exceptionHandlerPathFeature?.Path);
            }

            var payload = JsonSerializer.Serialize(new
            {
                error = "An unexpected error occurred.",
                detail = app.Environment.IsDevelopment() ? exception?.Message : null
            });

            await context.Response.WriteAsync(payload);
        });
    });

    // 3. Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";

        options.GetLevel = (context, elapsed, ex) =>
            ex != null || context.Response.StatusCode >= 500
                ? Serilog.Events.LogEventLevel.Error
                : Serilog.Events.LogEventLevel.Information;
    });

    // 4. Custom middleware
    app.UseCorrelationId();

    // 5. Swagger (always available for testing)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookMyShow API V1");
        c.RoutePrefix = string.Empty;
    });

    if (!app.Environment.IsDevelopment())
    {
        if (app.Environment.EnvironmentName != "Production")
        {
            app.UseHttpsRedirection();
        }
    }

    // 6. Correct required order: Routing -> Cors -> Authorization -> Endpoints
    app.UseRouting();

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