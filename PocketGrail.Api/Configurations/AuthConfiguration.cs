namespace PocketGrail.Api.Configurations;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Providers;
using PocketGrail.Application.Services;

public static class AuthConfiguration
{
    public static IServiceCollection AddAuthConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSecret   = Environment.GetEnvironmentVariable("JWT_SECRET")
            ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set.");
        var jwtIssuer   = Environment.GetEnvironmentVariable("JWT_ISSUER")
                          ?? configuration["Jwt:Issuer"]
                          ?? "PocketGrail";
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                          ?? configuration["Jwt:Audience"]
                          ?? "PocketGrailClient";

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = jwtIssuer,
                    ValidAudience            = jwtAudience,
                    IssuerSigningKey         = new SymmetricSecurityKey(
                                                  Encoding.UTF8.GetBytes(jwtSecret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("MySecretCookies", out var token))
                            context.Token = token;
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("DungeonMasterOnly", policy =>
                policy.RequireRole("DungeonMaster"));
            options.AddPolicy("PlayerAndAbove", policy =>
                policy.RequireRole("DungeonMaster", "Player"));
        });

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
