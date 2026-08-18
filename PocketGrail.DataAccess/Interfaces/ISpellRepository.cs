namespace PocketGrail.DataAccess.Interfaces;

using PocketGrail.DataAccess.Entities;

public interface ISpellRepository
{
    Task<IReadOnlyList<Spell>> GetAllAsync(CancellationToken ct = default);
    Task<Spell?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Spell> AddAsync(Spell spell, CancellationToken ct = default);
    Task DeleteAsync(Spell spell, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
