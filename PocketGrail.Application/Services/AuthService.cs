namespace PocketGrail.Application.Services;

using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Enums;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider    _jwtProvider;

    public AuthService(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider    = jwtProvider;
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

        return await _jwtProvider.GenerateTokenAsync(user);
    }

    public async Task<string> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = request.Email.ToLowerInvariant().Trim();

        var user = await _userRepository.GetByEmailAsync(email, ct)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await _jwtProvider.GenerateTokenAsync(user);
    }
}
