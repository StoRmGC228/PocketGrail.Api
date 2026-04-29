namespace PocketGrail.Infrastructure.Services;

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Configuration;
using Scriban;

public sealed class EmailService : IEmailService
{
    private readonly EmailConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailConfiguration> options, ILogger<EmailService> logger)
    {
        _config = options.Value;
        _logger = logger;
    }

    public async Task SendVerificationCodeAsync(string email, string username, string code, CancellationToken ct = default)
    {
        var htmlBody = await BuildHtmlBodyAsync(username, code, ct);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_config.SenderName, _config.SenderAddress));
        message.To.Add(new MailboxAddress(username, email));
        message.Subject = "Your PocketGrail verification code";
        message.Body = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = $"Your verification code is: {code}. It expires in 10 minutes."
        }.ToMessageBody();

        var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
            ?? throw new InvalidOperationException("SMTP_PASSWORD environment variable is missing.");

        using var client = new SmtpClient();
        await client.ConnectAsync(_config.SmtpHost, _config.SmtpPort, SecureSocketOptions.StartTls, ct);
        await client.AuthenticateAsync(_config.SmtpUsername, smtpPassword, ct);
        await client.SendAsync(message, ct);
        await client.DisconnectAsync(quit: true, ct);

        _logger.LogInformation("Verification code email sent to {Email}", email);
    }

    private async Task<string> BuildHtmlBodyAsync(string username, string code, CancellationToken ct)
    {
        if (!File.Exists(_config.VerificationCodeTemplatePath))
        {
            _logger.LogWarning("Email template not found at {Path}. Using fallback.", _config.VerificationCodeTemplatePath);
            return $"<p>Hello <strong>{username}</strong>,</p><p>Your verification code is: <strong>{code}</strong></p><p>It expires in 10 minutes.</p>";
        }

        var templateText = await File.ReadAllTextAsync(_config.VerificationCodeTemplatePath, ct);
        var template = Template.Parse(templateText);

        if (template.HasErrors)
        {
            _logger.LogWarning("Email template parsing failed at {Path}. Using fallback.", _config.VerificationCodeTemplatePath);
            return $"<p>Hello <strong>{username}</strong>,</p><p>Your verification code is: <strong>{code}</strong></p><p>It expires in 10 minutes.</p>";
        }

        return template.Render(new { username, code });
    }
}
