namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities.ClassEntities;

internal sealed class ClassRepository : IClassRepository
{
    private readonly PocketGrailDbContext _context;

    public ClassRepository(PocketGrailDbContext context) => _context = context;

    public Task<Class?> GetByNameAsync(string name, CancellationToken ct = default) =>
        _context.Classes
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower(), ct);

    public Task<Class?> GetByNameWithDetailsAsync(string name, CancellationToken ct = default) =>
        _context.Classes
            .Include(c => c.ClassFeatures)
                .ThenInclude(cf => cf.WeaponGrants)
            .Include(c => c.ClassFeatures)
                .ThenInclude(cf => cf.ArmorGrants)
            .Include(c => c.ClassFeatures)
                .ThenInclude(cf => cf.LanguageGrants)
            .Include(c => c.ClassFeatures)
                .ThenInclude(cf => cf.InstrumentGrants)
            .Include(c => c.SavingThrows)
            .Include(c => c.SpellSlotTemplates)
            .Include(c => c.MulticlassPrerequisites)
            .Include(c => c.Subclasses)
                .ThenInclude(s => s.SubclassFeatures)
                    .ThenInclude(sf => sf.WeaponGrants)
            .Include(c => c.Subclasses)
                .ThenInclude(s => s.SubclassFeatures)
                    .ThenInclude(sf => sf.ArmorGrants)
            .Include(c => c.Subclasses)
                .ThenInclude(s => s.SubclassFeatures)
                    .ThenInclude(sf => sf.LanguageGrants)
            .Include(c => c.Subclasses)
                .ThenInclude(s => s.SubclassFeatures)
                    .ThenInclude(sf => sf.InstrumentGrants)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower(), ct);

    public async Task<IReadOnlyList<Class>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Classes
            .Include(c => c.Subclasses)
            .Include(c => c.AvailableSkillChoices)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Subclass>> GetSubclassesForClassAsync(string className, CancellationToken ct = default) =>
        await _context.Subclasses
            .Where(s => s.SourceClass.Name.ToLower() == className.ToLower())
            .ToListAsync(ct);

    public Task<Subclass?> GetSubclassByIdAsync(int id, CancellationToken ct = default) =>
        _context.Subclasses
            .Include(s => s.SourceClass)
            .Include(s => s.SubclassFeatures)
                .ThenInclude(sf => sf.WeaponGrants)
            .Include(s => s.SubclassFeatures)
                .ThenInclude(sf => sf.ArmorGrants)
            .Include(s => s.SubclassFeatures)
                .ThenInclude(sf => sf.LanguageGrants)
            .Include(s => s.SubclassFeatures)
                .ThenInclude(sf => sf.InstrumentGrants)
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<ClassStartingItemSet?> GetStartingItemsForClassAsync(string className, CancellationToken ct = default) =>
        _context.ClassStartingItemSets
            .Include(s => s.ChoicePairs)
                .ThenInclude(p => p.OptionA)
            .Include(s => s.ChoicePairs)
                .ThenInclude(p => p.OptionB)
            .FirstOrDefaultAsync(s => s.Class.Name.ToLower() == className.ToLower(), ct);
}
