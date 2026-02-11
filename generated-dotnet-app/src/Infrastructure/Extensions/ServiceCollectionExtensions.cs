using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using System;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Infrastructure.Data.Core;

namespace BookMyShow.Infrastructure.Extensions
{
    /// <summary>
    /// Dependency injection extensions for Infrastructure layer
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Infrastructure layer services to the dependency injection container
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found");

            // Enable snake_case → PascalCase mapping for Dapper
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            // =====================================
            // Core Repositories Registration
            // =====================================

            services.AddScoped<IUsersRepository>(
                _ => new UsersRepository(connectionString));

            services.AddScoped<IMoviesRepository>(
                _ => new MoviesRepository(connectionString));

            services.AddScoped<ICitiesRepository>(
                _ => new CitiesRepository(connectionString));

            services.AddScoped<IStatesRepository>(
                _ => new StatesRepository(connectionString));

            services.AddScoped<ITheatresRepository>(
                _ => new TheatresRepository(connectionString));

            services.AddScoped<IScreensRepository>(
                _ => new ScreensRepository(connectionString));

            services.AddScoped<IShowsRepository>(
             _ => new ShowsRepository(configuration.GetConnectionString("DefaultConnection")!)
           );

            services.AddScoped<ISeatsRepository>(
                _ => new SeatsRepository(connectionString));

            services.AddScoped<IShowSeatsRepository>(
             _ => new ShowSeatsRepository(connectionString));    

            services.AddScoped<IBookingsRepository>(
            _ => new BookingsRepository(connectionString));

            services.AddScoped<IPaymentsRepository>(
            _ => new PaymentsRepository(connectionString));
           
            return services;
        }
    }
}
