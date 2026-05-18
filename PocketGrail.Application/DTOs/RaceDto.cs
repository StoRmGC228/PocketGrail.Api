namespace PocketGrail.Application.DTOs;

public sealed class RaceDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int BaseSpeed { get; init; }

    public int StrBonus { get; init; }
    public int DexBonus { get; init; }
    public int ConBonus { get; init; }
    public int IntBonus { get; init; }
    public int WisBonus { get; init; }
    public int ChaBonus { get; init; }
    public int FlexibleBonusPoints { get; init; }

    public IReadOnlyList<string> WeaponGrants { get; init; } = [];
    public IReadOnlyList<string> ArmorGrants { get; init; } = [];
    public IReadOnlyList<string> LanguageGrants { get; init; } = [];
    public IReadOnlyList<string> InstrumentGrants { get; init; } = [];
    public IReadOnlyList<RaceFeatureDto> Features { get; init; } = [];
}

public sealed class RaceFeatureDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
