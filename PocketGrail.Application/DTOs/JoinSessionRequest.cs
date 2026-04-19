namespace PocketGrail.Application.DTOs;

public sealed class JoinSessionRequest
{
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
}
