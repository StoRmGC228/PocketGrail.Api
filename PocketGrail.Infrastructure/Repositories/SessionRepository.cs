namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;

internal sealed class SessionRepository : ISessionRepository
{
    private readonly PocketGrailDbContext _context;

    public SessionRepository(PocketGrailDbContext context)
    {
        _context = context;
    }

    public Task<Session?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        _context.Sessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.Code == code, ct);

    public async Task<IReadOnlyList<Session>> GetActiveSessionsAsync(CancellationToken ct = default) =>
        await _context.Sessions
            .Include(s => s.Participants)
            .Where(s => s.IsActive)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(ct);

    public Task<bool> CodeExistsAsync(string code, CancellationToken ct = default) =>
        _context.Sessions.AnyAsync(s => s.Code == code, ct);

    public async Task AddAsync(Session session, CancellationToken ct = default) =>
        await _context.Sessions.AddAsync(session, ct);

    public async Task AddParticipantAsync(Participant participant, CancellationToken ct = default) =>
        await _context.Participants.AddAsync(participant, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
