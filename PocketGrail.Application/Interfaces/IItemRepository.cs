namespace PocketGrail.Application.Interfaces;

using PocketGrail.Domain.Entities.Characters;

public interface IItemRepository
{
    Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken ct = default);
    Task<Item?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Item> AddAsync(Item item, CancellationToken ct = default);
    Task DeleteAsync(Item item, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
