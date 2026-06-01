namespace PocketGrail.Api.Configuration;

using PocketGrail.Application.Configuration;

public static class ServicesConfiguration
{
    public static WebApplicationBuilder AddPocketGrailServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices(builder.Configuration);
        return builder;
    }
}
