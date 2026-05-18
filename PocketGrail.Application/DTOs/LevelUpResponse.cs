namespace PocketGrail.Application.DTOs;

public sealed class LevelUpResponse
{
    public bool RequiresAbilityScoreChoice { get; init; }
    public string? Message { get; init; }
    public CharacterDetailDto? Character { get; init; }
}
