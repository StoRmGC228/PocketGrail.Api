namespace PocketGrail.Application.Interfaces;

using PocketGrail.Application.DTOs;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<string> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<string> VerifyCodeAsync(VerifyCodeRequest request, CancellationToken ct = default);
}
