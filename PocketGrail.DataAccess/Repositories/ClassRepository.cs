namespace PocketGrail.DataAccess.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.DataAccess.Interfaces;
using PocketGrail.DataAccess.Entities.ClassEntities;
using PocketGrail.DataAccess.Entities.Enums;

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

    public Task<Class?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
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
            .Include(c => c.AvailableSkillChoices)
            .Include(c => c.Subclasses)
                .ThenInclude(s => s.SubclassFeatures)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

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

    public async Task<IReadOnlyList<ClassSavingThrowProficiency>> GetSavingThrowsAsync(string className, CancellationToken ct = default) =>
        await _context.ClassSavingThrowProficiencies
            .Where(st => st.Class.Name.ToLower() == className.ToLower())
            .ToListAsync(ct);

    public Task<ClassSavingThrowProficiency?> GetSavingThrowByIdAsync(int id, CancellationToken ct = default) =>
        _context.ClassSavingThrowProficiencies
            .Include(st => st.Class)
            .FirstOrDefaultAsync(st => st.Id == id, ct);

    public Task<bool> SavingThrowExistsAsync(int classId, Ability ability, CancellationToken ct = default) =>
        _context.ClassSavingThrowProficiencies
            .AnyAsync(st => st.ClassId == classId && st.Ability == ability, ct);

    public async Task AddSavingThrowAsync(ClassSavingThrowProficiency savingThrow, CancellationToken ct = default) =>
        await _context.ClassSavingThrowProficiencies.AddAsync(savingThrow, ct);

    public Task DeleteSavingThrowAsync(ClassSavingThrowProficiency savingThrow, CancellationToken ct = default)
    {
        _context.ClassSavingThrowProficiencies.Remove(savingThrow);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
