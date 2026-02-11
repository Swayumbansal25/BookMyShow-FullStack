# GeneratedApp - .NET Core Clean Architecture Application

This application was generated from your PostgreSQL database schema.

## Prerequisites

- .NET Core 9.0 SDK
- PostgreSQL database

## Setup

1. Update the connection string in `src/WebApi/appsettings.json`
2. Navigate to the WebApi directory: `cd src/WebApi`
3. Restore dependencies: `dotnet restore`
4. Build the application: `dotnet build`
5. Run the application: `dotnet run`

## Architecture

This application follows Clean Architecture principles:

- **Core**: Contains entities and repository interfaces (no dependencies)
- **Infrastructure**: Contains data access implementations using Dapper
- **Application**: Contains use cases and business logic (placeholder)
- **WebApi**: Contains controllers and API configuration

## API Documentation

When running in development mode, Swagger UI is available at:
`https://localhost:5001/swagger`

## Testing

- Unit tests: `tests/UnitTests/`
- Integration tests: `tests/IntegrationTests/`

Run tests with: `dotnet test`

## Generated with

[.NET Core App Generator](https://github.com/yourusername/netcore-generator)
