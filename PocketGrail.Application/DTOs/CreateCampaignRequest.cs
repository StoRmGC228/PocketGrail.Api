namespace PocketGrail.Application.DTOs;

using Microsoft.AspNetCore.Http;

public sealed class CreateCampaignRequest
{
    public string Name { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public IFormFile? Image { get; init; }
}
