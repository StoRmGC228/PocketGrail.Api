namespace PocketGrail.Application.DTOs;

public sealed class CampaignParticipantDto
{
    public int UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
