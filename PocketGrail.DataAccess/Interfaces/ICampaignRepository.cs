namespace PocketGrail.DataAccess.Interfaces;

using PocketGrail.DataAccess.Entities;

public interface ICampaignRepository
{
    Task<Campaign?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Campaign?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Campaign>> GetActiveAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Campaign>> GetByDmOwnerIdAsync(int dmUserId, CancellationToken ct = default);
    Task<IReadOnlyList<Campaign>> GetByParticipantUserIdAsync(int userId, CancellationToken ct = default);
    Task<bool> CodeExistsAsync(string code, CancellationToken ct = default);
    Task<bool> IsUserParticipantAsync(int campaignId, int userId, CancellationToken ct = default);
    Task AddAsync(Campaign campaign, CancellationToken ct = default);
    Task AddParticipantAsync(CampaignParticipant participant, CancellationToken ct = default);
    Task DeleteAsync(Campaign campaign, CancellationToken ct = default);
    Task RemoveParticipantAsync(int campaignId, int userId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
