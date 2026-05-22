namespace PocketGrail.Application.DTOs;

public sealed class AllyDto
{
    public int CharacterId { get; init; }
    public string CharacterName { get; init; } = string.Empty;
    public string Race { get; init; } = string.Empty;
    public IReadOnlyList<CharacterClassDto> Classes { get; init; } = [];
    public string ClassDisplay { get; init; } = string.Empty;
    public int Level { get; init; }
    public int CurrentHp { get; init; }
    public int MaxHp { get; init; }
    public string? ImageUrl { get; init; }
    public int UserId { get; init; }
    public string Username { get; init; } = string.Empty;
}
