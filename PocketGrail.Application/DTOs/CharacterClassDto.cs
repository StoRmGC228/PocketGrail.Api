namespace PocketGrail.Application.DTOs;

public sealed class CharacterClassDto
{
    public int Id { get; init; }
    public string ClassName { get; init; } = string.Empty;
    public int ClassLevel { get; init; }
    public string HitDice { get; init; } = string.Empty;
    public string? Subclass { get; init; }
    public int TotalHitDice { get; init; }
}
