namespace PocketGrail.Infrastructure.InfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PocketGrailDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString(
                    Environment.GetEnvironmentVariable("POCKET_GRAIL_CONNECTION_STRING"))));
        return services;
    }
}