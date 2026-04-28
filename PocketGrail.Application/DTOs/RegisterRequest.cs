namespace PocketGrail.Application.DTOs;

public sealed class RegisterRequest
{
    public string Email { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = "Player";
}
