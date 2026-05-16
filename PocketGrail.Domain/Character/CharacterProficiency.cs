namespace PocketGrail.Domain.Entities;

public class CharacterProficiency
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public int ProficiencyId { get; set; }
    public Proficiency Proficiency { get; set; } = null!;

    public bool HasExpertise { get; set; }
    public string? AbilityKey { get; set; }
}