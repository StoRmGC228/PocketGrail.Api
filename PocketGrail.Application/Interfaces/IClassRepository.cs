namespace PocketGrail.Application.Interfaces;

using PocketGrail.Domain.Entities.ClassEntities;

public interface IClassRepository
{
    Task<Class?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<Class?> GetByNameWithDetailsAsync(string name, CancellationToken ct = default);
    Task<IReadOnlyList<Class>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Subclass>> GetSubclassesForClassAsync(string className, CancellationToken ct = default);
    Task<Subclass?> GetSubclassByIdAsync(int id, CancellationToken ct = default);
    Task<ClassStartingItemSet?> GetStartingItemsForClassAsync(string className, CancellationToken ct = default);
}
