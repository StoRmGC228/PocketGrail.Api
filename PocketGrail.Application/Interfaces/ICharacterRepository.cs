namespace PocketGrail.Application.Interfaces;

using PocketGrail.Domain.Entities.Characters;

public interface ICharacterRepository
{
    Task<Character?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Character?> GetDetailByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Character>> GetByOwnerIdAsync(int ownerId, CancellationToken ct = default);
    Task<IReadOnlyList<Character>> GetCampaignCharactersAsync(int campaignId, CancellationToken ct = default);
    Task<IReadOnlyList<Item>> GetItemsByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
    Task AddAsync(Character character, CancellationToken ct = default);
    Task DeleteAsync(Character character, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}