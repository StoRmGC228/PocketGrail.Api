namespace PocketGrail.Application.DTOs;

public sealed class CharacterDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Race { get; init; } = string.Empty;
    public IReadOnlyList<CharacterClassDto> Classes { get; init; } = [];
    public string ClassDisplay { get; init; } = string.Empty;
    public int Level { get; init; }
    public int CurrentHp { get; init; }
    public int MaxHp { get; init; }
    public string? ImageUrl { get; init; }
    public int OwnerId { get; init; }
    public string OwnerUsername { get; init; } = string.Empty;
    public int? CampaignId { get; init; }
    public string? CampaignName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
