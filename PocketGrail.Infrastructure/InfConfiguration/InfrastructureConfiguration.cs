namespace PocketGrail.Infrastructure.InfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Services;
using PocketGrail.Infrastructure.Repositories;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("POCKET_GRAIL_CONNECTION_STRING")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string not configured. Set POCKET_GRAIL_CONNECTION_STRING env var or ConnectionStrings:DefaultConnection in appsettings.");

        services.AddDbContext<PocketGrailDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ISessionService, SessionService>();

        return services;
    }
}