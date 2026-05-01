namespace PocketGrail.Application.Services;

using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Enums;

public sealed class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _repository;
    private readonly ICloudinaryService _cloudinaryService;
    private const int MaxCodeGenerationAttempts = 10;

    public CampaignService(ICampaignRepository repository, ICloudinaryService cloudinaryService)
    {
        _repository = repository;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<CampaignDto> CreateCampaignAsync(
        CreateCampaignRequest request, int dmUserId, CancellationToken ct = default)
    {
        var code = await GenerateUniqueCodeAsync(ct);
        var now = DateTime.UtcNow;

        string? imageUrl = null;
        if (request.Image is not null)
            imageUrl = await _cloudinaryService.UploadImageAsync(request.Image, ct);

        var campaign = new Campaign
        {
            Name = request.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            ShortDescription = request.ShortDescription,
            ConnectionCode = code,
            ImageUrl = imageUrl,
            IsActive = true,
            DmOwnerId = dmUserId,
            CreatedAt = now,
            UpdatedAt = now
        };

        var dm = new CampaignParticipant
        {
            UserId = dmUserId,
            Role = UserRole.DungeonMaster,
            Campaign = campaign,
            CreatedAt = now,
            UpdatedAt = now
        };

        campaign.Participants.Add(dm);

        await _repository.AddAsync(campaign, ct);
        await _repository.SaveChangesAsync(ct);

        return MapToDto(campaign, includeParticipants: true);
    }

    public async Task<CampaignDto> JoinCampaignAsync(
        JoinCampaignRequest request, int userId, CancellationToken ct = default)
    {
        Campaign? campaign;

        if (request.ConnectionCode is not null)
            campaign = await _repository.GetByCodeAsync(request.ConnectionCode, ct);
        else if (request.CampaignId is not null)
            campaign = await _repository.GetByIdAsync(request.CampaignId.Value, ct);
        else
            throw new InvalidOperationException("Either ConnectionCode or CampaignId must be provided.");

        if (campaign is null)
            throw new KeyNotFoundException("Campaign not found.");

        if (!campaign.IsActive)
            throw new InvalidOperationException("Campaign is not active.");

        if (await _repository.IsUserParticipantAsync(campaign.Id, userId, ct))
            throw new InvalidOperationException("You are already a participant in this campaign.");

        // Only verify password when joining by campaign ID (browsing the campaigns list).
        // Joining by connection code (or share link) is passwordless — the 6-char code is the secret.
        if (request.CampaignId.HasValue)
        {
            if (string.IsNullOrEmpty(request.Password) || !BCrypt.Net.BCrypt.Verify(request.Password, campaign.PasswordHash))
                throw new UnauthorizedAccessException("Invalid campaign password.");
        }

        var now = DateTime.UtcNow;

        var participant = new CampaignParticipant
        {
            UserId = userId,
            Role = UserRole.Player,
            CampaignId = campaign.Id,
            // Do NOT set Campaign navigation property — setting it marks the already-tracked
            // Campaign entity as Modified, which causes Npgsql to throw DbUpdateConcurrencyException.
            CreatedAt = now,
            UpdatedAt = now
        };

        await _repository.AddParticipantAsync(participant, ct);
        await _repository.SaveChangesAsync(ct);

        campaign.Participants.Add(participant);

        return MapToDto(campaign, includeParticipants: true);
    }

    public async Task LeaveCampaignAsync(int campaignId, int userId, CancellationToken ct = default)
    {
        var campaign = await _repository.GetByIdAsync(campaignId, ct)
            ?? throw new KeyNotFoundException("Campaign not found.");

        if (campaign.DmOwnerId == userId)
            throw new InvalidOperationException("The campaign owner cannot leave their own campaign.");

        if (!await _repository.IsUserParticipantAsync(campaignId, userId, ct))
            throw new InvalidOperationException("You are not a participant in this campaign.");

        await _repository.RemoveParticipantAsync(campaignId, userId, ct);
        await _repository.SaveChangesAsync(ct);
    }

    public async Task DeleteCampaignAsync(int id, int dmUserId, CancellationToken ct = default)
    {
        var campaign = await _repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Campaign not found.");

        if (campaign.DmOwnerId != dmUserId)
            throw new UnauthorizedAccessException("Only the campaign owner can delete this campaign.");

        await _repository.DeleteAsync(campaign, ct);
        await _repository.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<CampaignDto>> GetActiveCampaignsAsync(CancellationToken ct = default)
    {
        var campaigns = await _repository.GetActiveAsync(ct);
        return campaigns.Select(c => MapToDto(c, includeParticipants: false)).ToList();
    }

    public async Task<IReadOnlyList<CampaignDto>> GetMyCampaignsAsync(
        int userId, string role, CancellationToken ct = default)
    {
        var campaigns = role == "DungeonMaster"
            ? await _repository.GetByDmOwnerIdAsync(userId, ct)
            : await _repository.GetByParticipantUserIdAsync(userId, ct);

        return campaigns.Select(c => MapToDto(c, includeParticipants: false)).ToList();
    }

    public async Task<CampaignDto?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        var campaign = await _repository.GetByCodeAsync(code, ct);
        return campaign is null ? null : MapToDto(campaign, includeParticipants: true);
    }

    public async Task<CampaignDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var campaign = await _repository.GetByIdAsync(id, ct);
        return campaign is null ? null : MapToDto(campaign, includeParticipants: true);
    }

    private async Task<string> GenerateUniqueCodeAsync(CancellationToken ct)
    {
        for (int attempt = 0; attempt < MaxCodeGenerationAttempts; attempt++)
        {
            var code = CodeGeneratorService.Generate();
            if (!await _repository.CodeExistsAsync(code, ct))
                return code;
        }

        throw new InvalidOperationException("Failed to generate a unique campaign code after multiple attempts.");
    }

    private static CampaignDto MapToDto(Campaign c, bool includeParticipants) => new()
    {
        Id = c.Id,
        Name = c.Name,
        ShortDescription = c.ShortDescription,
        ConnectionCode = c.ConnectionCode,
        ImageUrl = c.ImageUrl,
        IsActive = c.IsActive,
        DmOwnerId = c.DmOwnerId,
        DmOwnerUsername = c.DmOwner?.Username ?? string.Empty,
        ParticipantCount = c.Participants.Count,
        CreatedAt = c.CreatedAt,
        Participants = includeParticipants
            ? c.Participants.Select(MapParticipantToDto).ToList()
            : []
    };

    private static CampaignParticipantDto MapParticipantToDto(CampaignParticipant p) => new()
    {
        UserId = p.UserId,
        Username = p.User?.Username ?? string.Empty,
        Role = p.Role.ToString()
    };
}
