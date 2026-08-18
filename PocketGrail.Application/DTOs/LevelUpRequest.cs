namespace PocketGrail.Application.DTOs;

public sealed class LevelUpRequest
{
    public int? StrIncrease { get; init; }
    public int? DexIncrease { get; init; }
    public int? ConIncrease { get; init; }
    public int? IntIncrease { get; init; }
    public int? WisIncrease { get; init; }
    public int? ChaIncrease { get; init; }
    public AddFeatRequest? NewFeat { get; init; }
}
