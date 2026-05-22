namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;

internal sealed class SpellRepository : ISpellRepository
{
    private readonly PocketGrailDbContext _context;

    public SpellRepository(PocketGrailDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Spell>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Spells.OrderBy(s => s.Level).ThenBy(s => s.Name).ToListAsync(ct);

    public async Task<Spell?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Spells.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<Spell> AddAsync(Spell spell, CancellationToken ct = default)
    {
        await _context.Spells.AddAsync(spell, ct);
        return spell;
    }

    public Task DeleteAsync(Spell spell, CancellationToken ct = default)
    {
        _context.Spells.Remove(spell);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
