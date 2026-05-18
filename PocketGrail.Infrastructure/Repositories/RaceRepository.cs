namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;

internal sealed class RaceRepository : IRaceRepository
{
    private readonly PocketGrailDbContext _context;

    public RaceRepository(PocketGrailDbContext context) => _context = context;

    public async Task<IReadOnlyList<Race>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Races
            .Include(r => r.Features)
            .Include(r => r.WeaponGrants)
            .Include(r => r.ArmorGrants)
            .Include(r => r.LanguageGrants)
            .Include(r => r.InstrumentGrants)
            .AsSplitQuery()
            .OrderBy(r => r.Name)
            .ToListAsync(ct);

    public Task<Race?> GetByNameWithDetailsAsync(string name, CancellationToken ct = default) =>
        _context.Races
            .Include(r => r.Features)
            .Include(r => r.WeaponGrants)
            .Include(r => r.ArmorGrants)
            .Include(r => r.LanguageGrants)
            .Include(r => r.InstrumentGrants)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower(), ct);
}
