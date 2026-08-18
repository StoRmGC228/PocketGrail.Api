namespace PocketGrail.Application.DTOs;

public sealed class SkillProficiencyDto
{
    public int Id { get; init; }
    public string Skill { get; init; } = string.Empty;
    public bool HasExpertise { get; init; }
}
