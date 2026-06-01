namespace PocketGrail.DataAccess.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PocketGrail.DataAccess.Interfaces;
using PocketGrail.DataAccess.Repositories;

public static class DataAccessConfiguration
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("POCKET_GRAIL_CONNECTION_STRING")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string not configured. Set POCKET_GRAIL_CONNECTION_STRING env var.");

        services.AddDbContext<PocketGrailDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IRaceRepository, RaceRepository>();
        services.AddScoped<ISpellRepository, SpellRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
