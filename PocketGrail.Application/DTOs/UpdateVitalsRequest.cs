namespace PocketGrail.Application.DTOs;

public sealed class UpdateVitalsRequest
{
    public int? CurrentHp { get; init; }
    public int? MaxHp { get; init; }
    public int? TempHp { get; init; }
    public int? XpPoints { get; init; }
    public bool? HasInspiration { get; init; }
    public int? Exhaustion { get; init; }
    public int? DeathSuccesses { get; init; }
    public int? DeathFailures { get; init; }
}
