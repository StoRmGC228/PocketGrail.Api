namespace PocketGrail.Application.Interfaces;

using PocketGrail.Domain.Entities;

public interface ISessionRepository
{
    Task<Session?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Session>> GetActiveSessionsAsync(CancellationToken ct = default);
    Task<bool> CodeExistsAsync(string code, CancellationToken ct = default);
    Task AddAsync(Session session, CancellationToken ct = default);
    Task AddParticipantAsync(Participant participant, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
