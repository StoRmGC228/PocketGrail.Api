namespace PocketGrail.Application.DTOs;

public sealed class UpdateCharacterClassRequest
{
    public string? Subclass { get; init; }
    public int? UsedHitDice { get; init; }
}
