namespace PocketGrail.Application.DTOs;

public sealed class JoinCampaignRequest
{
    public string? ConnectionCode { get; init; }
    public int? CampaignId { get; init; }
    // Password is only required when joining by CampaignId (browse list).
    // Joining by ConnectionCode (or share link) is passwordless — the code is the secret.
    public string? Password { get; init; }
}
