namespace PocketGrail.Application.Interfaces;

using PocketGrail.DataAccess.Entities;

public interface IJwtProvider
{
    Task<string> GenerateTokenAsync(User user);
}
