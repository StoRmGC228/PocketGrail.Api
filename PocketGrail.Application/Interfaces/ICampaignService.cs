namespace PocketGrail.Application.Interfaces;

using PocketGrail.Application.DTOs;

public interface ICampaignService
{
    Task<CampaignDto> CreateCampaignAsync(CreateCampaignRequest request, int dmUserId, CancellationToken ct = default);
    Task<CampaignDto> JoinCampaignAsync(JoinCampaignRequest request, int userId, CancellationToken ct = default);
    Task DeleteCampaignAsync(int id, int dmUserId, CancellationToken ct = default);
    Task LeaveCampaignAsync(int campaignId, int userId, CancellationToken ct = default);
    Task<IReadOnlyList<CampaignDto>> GetActiveCampaignsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CampaignDto>> GetMyCampaignsAsync(int userId, string role, CancellationToken ct = default);
    Task<CampaignDto?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<CampaignDto?> GetByIdAsync(int id, CancellationToken ct = default);
}
