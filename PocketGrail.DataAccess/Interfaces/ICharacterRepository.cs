namespace PocketGrail.DataAccess.Interfaces;

using PocketGrail.DataAccess.Entities;
using PocketGrail.DataAccess.Entities.Characters;

public interface ICharacterRepository
{
    Task<Character?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Character?> GetDetailByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Character>> GetByOwnerIdAsync(int ownerId, CancellationToken ct = default);
    Task<IReadOnlyList<Character>> GetCampaignCharactersAsync(int campaignId, CancellationToken ct = default);
    Task<IReadOnlyList<Item>> GetItemsByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
    Task LinkItemAsync(int characterId, int itemId, CancellationToken ct = default);
    Task LinkSpellAsync(int characterId, int spellId, CancellationToken ct = default);
    Task AddAsync(Character character, CancellationToken ct = default);
    Task DeleteAsync(Character character, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}