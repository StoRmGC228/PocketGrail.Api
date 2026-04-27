namespace PocketGrail.Application.DTOs;

public sealed class ParticipantDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public DateTime JoinedAt { get; init; }
}
