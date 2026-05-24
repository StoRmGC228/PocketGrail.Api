namespace PocketGrail.Domain.Entities;

using PocketGrail.Domain.Enums;
using PocketGrail.Domain.Exceptions;

public sealed class User
{
    public int Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }

    private User() { }

    public static User Reconstitute(int id, string username, string email, string passwordHash, UserRole role) =>
        new() { Id = id, Username = username, Email = email, PasswordHash = passwordHash, Role = role };

    public static User Create(string username, string email, string passwordHash, UserRole role) =>
        new() { Username = username, Email = email, PasswordHash = passwordHash, Role = role };

    public void SetPasswordHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash)) throw new DomainException("Password hash cannot be empty.");
        PasswordHash = hash;
    }

    public void UpdateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new DomainException("Username cannot be empty.");
        Username = username;
    }

    public void SetRole(UserRole role) => Role = role;
}
