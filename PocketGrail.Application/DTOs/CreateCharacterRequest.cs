namespace PocketGrail.Application.DTOs;

using Microsoft.AspNetCore.Http;

public sealed class CreateCharacterRequest
{
    public string Name { get; init; } = string.Empty;
    public string Race { get; init; } = string.Empty;
    public string ClassName { get; init; } = string.Empty;
    public int? CampaignId { get; init; }
    public IFormFile? Image { get; init; }
}
