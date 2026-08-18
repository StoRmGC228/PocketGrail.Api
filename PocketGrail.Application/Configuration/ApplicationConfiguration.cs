namespace PocketGrail.Application.Configuration;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Providers;
using PocketGrail.Application.Services;
using PocketGrail.DataAccess.Configuration;
using PocketGrail.Infrastructure.Configuration;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDataAccess(configuration);
        services.AddInfrastructure(configuration);
        services.AddAuthConfiguration(configuration);

        services.AddMemoryCache();

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICampaignService, CampaignService>();
        services.AddScoped<ICharacterService, CharacterService>();
        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        return services;
    }
}
