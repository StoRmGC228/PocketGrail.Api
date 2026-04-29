namespace PocketGrail.Api.Configuration;

using PocketGrail.Application.Configuration;
using PocketGrail.Infrastructure.InfConfiguration;

public static class ServicesConfiguration
{
    public static WebApplicationBuilder AddPocketGrailServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddAuthConfiguration(builder.Configuration);
        return builder;
    }
}
