namespace PocketGrail.Application.DTOs;

public sealed class CatalogSpellDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; }
    public string? School { get; init; }
    public string? Range { get; init; }
    public string? CastingTime { get; init; }
    public bool Concentration { get; init; }
    public bool IsRitual { get; init; }
    public string? Components { get; init; }
}
