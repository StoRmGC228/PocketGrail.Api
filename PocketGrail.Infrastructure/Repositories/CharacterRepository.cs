namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities.Characters;

internal sealed class CharacterRepository : ICharacterRepository
{
    private readonly PocketGrailDbContext _context;

    public CharacterRepository(PocketGrailDbContext context)
    {
        _context = context;
    }

    public Task<Character?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Characters
            .Include(c => c.Owner)
            .Include(c => c.Campaign)
            .Include(c => c.Classes).ThenInclude(cc => cc.Class)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Character?> GetDetailByIdAsync(int id, CancellationToken ct = default) =>
        _context.Characters
            .Include(c => c.Owner)
            .Include(c => c.Campaign)
            .Include(c => c.Classes).ThenInclude(cc => cc.Class).ThenInclude(cl => cl.SavingThrows)
            .Include(c => c.Classes).ThenInclude(cc => cc.CharacterSubclass)
            .Include(c => c.CharacterStats)
            .Include(c => c.Wallet)
            .Include(c => c.SpellSlots)
            .Include(c => c.Items).ThenInclude(i => i.CharacterItems.Where(ci => ci.CharacterId == id))
            .Include(c => c.Spells).ThenInclude(s => s.CharacterSpells.Where(cs => cs.CharacterId == id))
            .Include(c => c.Feats)
            .Include(c => c.Features)
            .Include(c => c.Proficiencies).ThenInclude(cp => cp.Skills)
            .Include(c => c.Proficiencies).ThenInclude(cp => cp.AdditionalSavingThrows)
            .Include(c => c.Proficiencies).ThenInclude(cp => cp.Languages)
            .Include(c => c.Proficiencies).ThenInclude(cp => cp.Instruments)
            .Include(c => c.Proficiencies).ThenInclude(cp => cp.Weapons)
            .Include(c => c.Proficiencies).ThenInclude(cp => cp.Armors)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<Character>> GetByOwnerIdAsync(int ownerId, CancellationToken ct = default) =>
        await _context.Characters
            .Include(c => c.Owner)
            .Include(c => c.Campaign)
            .Include(c => c.Classes).ThenInclude(cc => cc.Class)
            .Where(c => c.OwnerId == ownerId)
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Character>> GetCampaignCharactersAsync(int campaignId, CancellationToken ct = default) =>
        await _context.Characters
            .Include(c => c.Owner)
            .Include(c => c.Classes).ThenInclude(cc => cc.Class)
            .Where(c => c.CampaignId == campaignId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Item>> GetItemsByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default) =>
        await _context.Items.Where(i => ids.Contains(i.Id)).ToListAsync(ct);

    public async Task LinkItemAsync(int characterId, int itemId, CancellationToken ct = default)
    {
        var junction = new CharacterItem
        {
            CharacterId = characterId,
            ItemId = itemId,
            Quantity = 1,
            IsEquipped = false,
            IsAttuned = false,
        };
        await _context.CharacterItems.AddAsync(junction, ct);
    }

    public async Task LinkSpellAsync(int characterId, int spellId, CancellationToken ct = default)
    {
        var junction = new CharacterSpell
        {
            CharacterId = characterId,
            SpellId = spellId,
            Prepared = false,
        };
        await _context.CharacterSpells.AddAsync(junction, ct);
    }

    public async Task AddAsync(Character character, CancellationToken ct = default) =>
        await _context.Characters.AddAsync(character, ct);

    public Task DeleteAsync(Character character, CancellationToken ct = default)
    {
        _context.Characters.Remove(character);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
