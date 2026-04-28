namespace PocketGrail.Application.Interfaces;

using PocketGrail.Domain.Entities;

public interface IJwtProvider
{
    Task<string> GenerateTokenAsync(User user);
}
