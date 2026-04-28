namespace PocketGrail.Application.DTOs;

public sealed class JoinSessionRequest
{
    public int UserId { get; init; }
    public string Code { get; init; } = string.Empty;
}
