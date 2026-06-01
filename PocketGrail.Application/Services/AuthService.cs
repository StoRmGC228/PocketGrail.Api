namespace PocketGrail.Application.Services;

using Microsoft.Extensions.Caching.Memory;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Helpers;
using PocketGrail.Application.Interfaces;
using PocketGrail.DataAccess.Entities;
using PocketGrail.DataAccess.Entities.Enums;
using PocketGrail.DataAccess.Interfaces;
using PocketGrail.Infrastructure.Interfaces;

public sealed class AuthService : IAuthService
{
    private static readonly TimeSpan CodeExpiry = TimeSpan.FromMinutes(10);

    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider    _jwtProvider;
    private readonly IEmailService   _emailService;
    private readonly IMemoryCache    _cache;

    public AuthService(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IEmailService emailService,
        IMemoryCache cache)
    {
        _userRepository = userRepository;
        _jwtProvider    = jwtProvider;
        _emailService   = emailService;
        _cache          = cache;
    }

    public async Task<string> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var email = request.Email.ToLowerInvariant().Trim();

        if (await _userRepository.ExistsAsync(email, ct))
            throw new InvalidOperationException($"A user with email '{email}' already exists.");

        var role = Enum.TryParse<UserRole>(request.Role, ignoreCase: true, out var parsed)
            ? parsed
            : UserRole.Player;

        var now  = DateTime.UtcNow;
        var user = new User
        {
            Email        = email,
            Username     = request.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role         = role,
            CreatedAt    = now,
            UpdatedAt    = now
        };

        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        await SendCodeAsync(email, user.Username, ct);
        return email;
    }

    public async Task<string> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = request.Email.ToLowerInvariant().Trim();

        var user = await _userRepository.GetByEmailAsync(email, ct)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        await SendCodeAsync(email, user.Username, ct);
        return email;
    }

    public async Task<string> VerifyCodeAsync(VerifyCodeRequest request, CancellationToken ct = default)
    {
        var email    = request.Email.ToLowerInvariant().Trim();
        var cacheKey = CacheKeys.VerificationCode(email);

        if (!_cache.TryGetValue<string>(cacheKey, out var storedCode) || storedCode != request.Code)
            throw new UnauthorizedAccessException("Invalid or expired verification code.");

        _cache.Remove(cacheKey);

        var user = await _userRepository.GetByEmailAsync(email, ct)
            ?? throw new UnauthorizedAccessException("User not found.");

        return await _jwtProvider.GenerateTokenAsync(user);
    }

    private async Task SendCodeAsync(string email, string username, CancellationToken ct)
    {
        var code = CodeGeneratorService.Generate();
        _cache.Set(CacheKeys.VerificationCode(email), code, CodeExpiry);
        await _emailService.SendVerificationCodeAsync(email, username, code, ct);
    }
}
