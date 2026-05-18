namespace PocketGrail.Application.DTOs;

public sealed class ClassDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string HitDice { get; init; } = string.Empty;
    public string? SpellAbility { get; init; }
    public int SkillChoiceCount { get; init; }
    public IReadOnlyList<string> AvailableSkillChoices { get; init; } = [];
    public IReadOnlyList<SubclassDto> Subclasses { get; init; } = [];
}
