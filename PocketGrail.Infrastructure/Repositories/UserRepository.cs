namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;

internal sealed class UserRepository : IUserRepository
{
    private readonly PocketGrailDbContext _context;

    public UserRepository(PocketGrailDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<bool> ExistsAsync(string email, CancellationToken ct = default) =>
        _context.Users.AnyAsync(u => u.Email == email, ct);

    public async Task AddAsync(User user, CancellationToken ct = default) =>
        await _context.Users.AddAsync(user, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
