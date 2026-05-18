namespace PocketGrail.Application.DTOs;

public sealed class StartingItemDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

public sealed class StartingItemChoicePairDto
{
    public int Id { get; init; }
    public IReadOnlyList<StartingItemDto> OptionA { get; init; } = [];
    public IReadOnlyList<StartingItemDto> OptionB { get; init; } = [];
}

public sealed class ClassStartingItemSetDto
{
    public IReadOnlyList<StartingItemChoicePairDto> ChoicePairs { get; init; } = [];
}
