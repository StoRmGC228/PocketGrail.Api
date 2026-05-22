namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities.Characters;

internal sealed class ItemRepository : IItemRepository
{
    private readonly PocketGrailDbContext _context;

    public ItemRepository(PocketGrailDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Items.OrderBy(i => i.Name).ToListAsync(ct);

    public async Task<Item?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Items.FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<Item> AddAsync(Item item, CancellationToken ct = default)
    {
        await _context.Items.AddAsync(item, ct);
        return item;
    }

    public Task DeleteAsync(Item item, CancellationToken ct = default)
    {
        _context.Items.Remove(item);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
