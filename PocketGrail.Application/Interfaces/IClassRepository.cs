namespace PocketGrail.Application.Interfaces;

using PocketGrail.Domain.Entities.ClassEntities;
using PocketGrail.Domain.Entities.Enums;

public interface IClassRepository
{
    Task<Class?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<Class?> GetByNameWithDetailsAsync(string name, CancellationToken ct = default);
    Task<IReadOnlyList<Class>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Subclass>> GetSubclassesForClassAsync(string className, CancellationToken ct = default);
    Task<Subclass?> GetSubclassByIdAsync(int id, CancellationToken ct = default);
    Task<ClassStartingItemSet?> GetStartingItemsForClassAsync(string className, CancellationToken ct = default);

    Task<IReadOnlyList<ClassSavingThrowProficiency>> GetSavingThrowsAsync(string className, CancellationToken ct = default);
    Task<ClassSavingThrowProficiency?> GetSavingThrowByIdAsync(int id, CancellationToken ct = default);
    Task<bool> SavingThrowExistsAsync(int classId, Ability ability, CancellationToken ct = default);
    Task AddSavingThrowAsync(ClassSavingThrowProficiency savingThrow, CancellationToken ct = default);
    Task DeleteSavingThrowAsync(ClassSavingThrowProficiency savingThrow, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
