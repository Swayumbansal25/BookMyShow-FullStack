using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.Services.Core;

namespace BookMyShow.Application.Extensions
{
    /// <summary>
    /// Dependency injection extensions for Application layer
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Application layer services to the dependency injection container
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // ============================
            // Core Application Services
            // ============================

             services.AddScoped<IUsersService, UsersService>();
             services.AddScoped<IMoviesService, MoviesService>();
             services.AddScoped<ICitiesService, CitiesService>();
             services.AddScoped<IStatesService, StatesService>();
             services.AddScoped<ITheatresService, TheatresService>();
             services.AddScoped<IScreensService, ScreensService>();
             services.AddScoped<IShowsService, ShowsService>();
             services.AddScoped<ISeatsService, SeatsService>();
             services.AddScoped<IShowSeatsService, ShowSeatsService>();
             services.AddScoped<IBookingsService, BookingsService>();
             services.AddScoped<IPaymentsService, PaymentsService>();


            // ============================
            // FluentValidation
            // ============================
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // ============================
            // AutoMapper
            // ============================
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            services.AddSingleton(sp => mapperConfiguration.CreateMapper());

            return services;
        }
    }
}
