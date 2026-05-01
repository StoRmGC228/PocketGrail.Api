namespace PocketGrail.Application.DTOs;

public sealed class CampaignDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string ConnectionCode { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
    public bool IsActive { get; init; }
    public int DmOwnerId { get; init; }
    public string DmOwnerUsername { get; init; } = string.Empty;
    public int ParticipantCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<CampaignParticipantDto> Participants { get; init; } = [];
}
