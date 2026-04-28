namespace PocketGrail.Application.DTOs;

public sealed class ParticipantDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Role { get; init; } = string.Empty;
}
