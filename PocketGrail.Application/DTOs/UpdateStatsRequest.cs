namespace PocketGrail.Application.DTOs;

public sealed class UpdateStatsRequest
{
    public int? StrScore { get; init; }
    public int? DexScore { get; init; }
    public int? ConScore { get; init; }
    public int? IntScore { get; init; }
    public int? WisScore { get; init; }
    public int? ChaScore { get; init; }
    public int? ArmorClass { get; init; }
    public int? Speed { get; init; }
    public string? SpellAbility { get; init; }
    public string? Alignment { get; init; }
}
