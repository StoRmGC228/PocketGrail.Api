namespace PocketGrail.Application.DTOs;

public sealed class ProficiencyDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ProficiencyType { get; init; } = "weapon";
    public bool HasExpertise { get; init; }
    public string? AbilityKey { get; init; }
}
