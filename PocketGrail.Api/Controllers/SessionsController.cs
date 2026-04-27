namespace PocketGrail.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public sealed class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    // POST /api/sessions
    [HttpPost]
    public async Task<IActionResult> CreateSession(
        [FromBody] CreateSessionRequest request,
        CancellationToken ct)
    {
        var session = await _sessionService.CreateSessionAsync(request, ct);
        return CreatedAtAction(nameof(GetByCode), new { code = session.Code }, session);
    }

    // POST /api/sessions/join
    [HttpPost("join")]
    public async Task<IActionResult> JoinSession(
        [FromBody] JoinSessionRequest request,
        CancellationToken ct)
    {
        try
        {
            var (session, participant) = await _sessionService.JoinSessionAsync(request, ct);
            return Ok(new { session, participant });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // GET /api/sessions
    [HttpGet]
    public async Task<IActionResult> GetActiveSessions(CancellationToken ct)
    {
        var sessions = await _sessionService.GetActiveSessionsAsync(ct);
        return Ok(sessions);
    }

    // GET /api/sessions/{code}
    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code, CancellationToken ct)
    {
        var session = await _sessionService.GetSessionByCodeAsync(code, ct);
        return session is null ? NotFound() : Ok(session);
    }

    // DELETE /api/sessions/{code}/leave/{participantId}
    [HttpDelete("{code}/leave/{participantId:guid}")]
    public async Task<IActionResult> LeaveSession(
        string code,
        Guid participantId,
        CancellationToken ct)
    {
        var result = await _sessionService.LeaveSessionAsync(participantId, code, ct);
        return result ? NoContent() : NotFound();
    }
}
