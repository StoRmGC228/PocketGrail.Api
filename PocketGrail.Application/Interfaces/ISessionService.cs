namespace PocketGrail.Application.Interfaces;

using PocketGrail.Application.DTOs;

public interface ISessionService
{
    Task<SessionDto> CreateSessionAsync(CreateSessionRequest request, CancellationToken ct = default);
    Task<(SessionDto session, ParticipantDto participant)> JoinSessionAsync(JoinSessionRequest request, CancellationToken ct = default);
    Task<SessionDto?> GetSessionByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<SessionDto>> GetActiveSessionsAsync(CancellationToken ct = default);
    Task<bool> LeaveSessionAsync(Guid participantId, string code, CancellationToken ct = default);
}
