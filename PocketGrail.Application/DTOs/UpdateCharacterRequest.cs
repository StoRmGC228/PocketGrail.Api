namespace PocketGrail.Application.DTOs;

using Microsoft.AspNetCore.Http;

public sealed class UpdateCharacterRequest
{
    public string? Name { get; init; }
    public string? Race { get; init; }
    public int? CurrentHp { get; init; }
    public int? MaxHp { get; init; }
    public int? CampaignId { get; init; }
    public IFormFile? Image { get; init; }
    public string? Alignment { get; init; }
    public string? SpellAbility { get; init; }
    public string? BackgroundStory { get; init; }
    public string? Appearance { get; init; }
    public string? Notes { get; init; }
}
