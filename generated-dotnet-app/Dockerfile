# Stage 1: Build environment using modern .NET 10 SDK on Ubuntu
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all .csproj files preserving folder structure to maximize Docker layer caching
COPY src/WebApi/WebApi.csproj src/WebApi/
COPY src/Application/Application.csproj src/Application/
COPY src/Core/Core.csproj src/Core/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/

# Restore dependencies
RUN dotnet restore src/WebApi/WebApi.csproj

# Copy everything else and build the production binaries
COPY . .
RUN dotnet publish src/WebApi/WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime environment using slim .NET 10 ASP.NET engine
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose Render's dynamic application port variable
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApi.dll"]