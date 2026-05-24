namespace PocketGrail.Infrastructure.Configuration;

using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PocketGrail.Infrastructure.Interfaces;
using PocketGrail.Infrastructure.Services;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailConfiguration>(opt =>
        {
            opt.SenderAddress = Environment.GetEnvironmentVariable("EMAIL_SENDER_ADDRESS")
                ?? throw new InvalidOperationException("EMAIL_SENDER_ADDRESS environment variable is missing.");
            opt.SenderName = Environment.GetEnvironmentVariable("EMAIL_SENDER_NAME")
                ?? throw new InvalidOperationException("EMAIL_SENDER_NAME environment variable is missing.");
            opt.SmtpHost = Environment.GetEnvironmentVariable("SMTP_HOST")
                ?? throw new InvalidOperationException("SMTP_HOST environment variable is missing.");
            opt.SmtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var port) ? port : 587;
            opt.SmtpUsername = Environment.GetEnvironmentVariable("SMTP_USERNAME")
                ?? throw new InvalidOperationException("SMTP_USERNAME environment variable is missing.");
            opt.VerificationCodeTemplatePath = configuration["Email:VerificationCodeTemplatePath"]
                ?? "Templates/verification_code.html";
        });
        services.AddScoped<IEmailService, EmailService>();

        var cloudinaryAccount = new Account(
            Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME")
                ?? throw new InvalidOperationException("CLOUDINARY_CLOUD_NAME environment variable is missing."),
            Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY")
                ?? throw new InvalidOperationException("CLOUDINARY_API_KEY environment variable is missing."),
            Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
                ?? throw new InvalidOperationException("CLOUDINARY_API_SECRET environment variable is missing.")
        );
        services.AddSingleton(new Cloudinary(cloudinaryAccount));
        services.AddScoped<ICloudinaryService, CloudinaryService>();

        return services;
    }
}
