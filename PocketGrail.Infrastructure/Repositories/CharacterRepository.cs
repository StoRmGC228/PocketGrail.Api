namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;

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
            .Include(c => c.Classes)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Character?> GetDetailByIdAsync(int id, CancellationToken ct = default) =>
        _context.Characters
            .Include(c => c.Owner)
            .Include(c => c.Campaign)
            .Include(c => c.Classes)
            .Include(c => c.Wallet)
            .Include(c => c.SpellSlots)
            .Include(c => c.Items).ThenInclude(i => i.CharacterItems.Where(ci => ci.CharacterId == id))
            .Include(c => c.Spells).ThenInclude(s => s.CharacterSpells.Where(cs => cs.CharacterId == id))
            .Include(c => c.Feats)
            .Include(c => c.Features).ThenInclude(f => f.CharacterFeatures.Where(cf => cf.CharacterId == id))
            .Include(c => c.Proficiencies).ThenInclude(p => p.CharacterProficiencies.Where(cp => cp.CharacterId == id))
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<Character>> GetByOwnerIdAsync(int ownerId, CancellationToken ct = default) =>
        await _context.Characters
            .Include(c => c.Owner)
            .Include(c => c.Campaign)
            .Include(c => c.Classes)
            .Where(c => c.OwnerId == ownerId)
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Character>> GetCampaignCharactersAsync(int campaignId, CancellationToken ct = default) =>
        await _context.Characters
            .Include(c => c.Owner)
            .Include(c => c.Classes)
            .Where(c => c.CampaignId == campaignId)
            .ToListAsync(ct);

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
