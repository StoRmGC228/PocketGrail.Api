namespace PocketGrail.Domain.SupportingTypes;

using PocketGrail.Domain.Enums;

public sealed record AsiOrFeatChoice
{
    public bool IsAsi { get; init; }

    public Ability? SingleAbility { get; init; }
    public int?     SingleBonus   { get; init; }

    public Ability? AbilityA { get; init; }
    public Ability? AbilityB { get; init; }

    public int? FeatId { get; init; }
}
