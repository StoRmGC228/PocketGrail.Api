namespace PocketGrail.Application.Services;

using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Enums;

public sealed class SessionService : ISessionService
{
    private readonly ISessionRepository _repository;
    private const int MaxCodeGenerationAttempts = 10;

    public SessionService(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<SessionDto> CreateSessionAsync(CreateSessionRequest request, CancellationToken ct = default)
    {
        var code = await GenerateUniqueCodeAsync(ct);
        var now = DateTime.UtcNow;

        var session = new Session
        {
            Id = Guid.NewGuid(),
            Code = code,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        var dm = new Participant
        {
            Id = Guid.NewGuid(),
            Name = request.DungeonMasterName,
            Role = UserRole.DungeonMaster,
            SessionId = session.Id,
            Session = session,
            JoinedAt = now,
            CreatedAt = now,
            UpdatedAt = now
        };

        session.Participants.Add(dm);

        await _repository.AddAsync(session, ct);
        await _repository.SaveChangesAsync(ct);

        return MapToDto(session);
    }

    public async Task<(SessionDto session, ParticipantDto participant)> JoinSessionAsync(
        JoinSessionRequest request, CancellationToken ct = default)
    {
        var session = await _repository.GetByCodeAsync(request.Code, ct)
            ?? throw new InvalidOperationException($"Session with code '{request.Code}' not found.");

        if (!session.IsActive)
            throw new InvalidOperationException("Session is not active.");

        var now = DateTime.UtcNow;

        var participant = new Participant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Role = UserRole.Player,
            SessionId = session.Id,
            // Do NOT set Session = session — that marks the already-tracked Session
            // entity as Modified, causing EF to batch an UPDATE alongside the INSERT.
            // Npgsql's aggregate row-count check then throws DbUpdateConcurrencyException
            // when the session row isn't actually changed. FK alone is sufficient.
            JoinedAt = now,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _repository.AddParticipantAsync(participant, ct);
        await _repository.SaveChangesAsync(ct);

        // Attach to the in-memory collection so MapToDto includes the new participant
        session.Participants.Add(participant);

        return (MapToDto(session), MapParticipantToDto(participant));
    }

    public async Task<SessionDto?> GetSessionByCodeAsync(string code, CancellationToken ct = default)
    {
        var session = await _repository.GetByCodeAsync(code, ct);
        return session is null ? null : MapToDto(session);
    }

    public async Task<IReadOnlyList<SessionDto>> GetActiveSessionsAsync(CancellationToken ct = default)
    {
        var sessions = await _repository.GetActiveSessionsAsync(ct);
        return sessions.Select(MapToDto).ToList();
    }

    public async Task<bool> LeaveSessionAsync(Guid participantId, string code, CancellationToken ct = default)
    {
        var session = await _repository.GetByCodeAsync(code, ct);
        if (session is null) return false;

        var participant = session.Participants.FirstOrDefault(p => p.Id == participantId);
        if (participant is null) return false;

        session.Participants.Remove(participant);
        session.UpdatedAt = DateTime.UtcNow;

        if (session.Participants.Count == 0)
            session.IsActive = false;

        await _repository.SaveChangesAsync(ct);
        return true;
    }

    private async Task<string> GenerateUniqueCodeAsync(CancellationToken ct)
    {
        for (int attempt = 0; attempt < MaxCodeGenerationAttempts; attempt++)
        {
            var code = CodeGeneratorService.Generate();
            if (!await _repository.CodeExistsAsync(code, ct))
                return code;
        }

        throw new InvalidOperationException("Failed to generate a unique session code after multiple attempts.");
    }

    private static SessionDto MapToDto(Session session) => new()
    {
        Id = session.Id,
        Code = session.Code,
        IsActive = session.IsActive,
        CreatedAt = session.CreatedAt,
        Participants = session.Participants.Select(MapParticipantToDto).ToList()
    };

    private static ParticipantDto MapParticipantToDto(Participant p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Role = p.Role.ToString(),
        JoinedAt = p.JoinedAt
    };
}
