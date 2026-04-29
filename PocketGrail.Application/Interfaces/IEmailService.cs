namespace PocketGrail.Application.Interfaces;

public interface IEmailService
{
    Task SendVerificationCodeAsync(string email, string username, string code, CancellationToken ct = default);
}
