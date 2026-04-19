namespace PocketGrail.Application.DTOs;

public sealed class SessionDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<ParticipantDto> Participants { get; init; } = [];
}
