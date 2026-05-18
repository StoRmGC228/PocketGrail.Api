namespace PocketGrail.Application.DTOs;

public sealed class SkillProficiencyDto
{
    public string Skill { get; init; } = string.Empty;
    public bool HasExpertise { get; init; }
}
