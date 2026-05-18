namespace PocketGrail.Domain.Entities.Proficiencies;

using PocketGrail.Domain.Entities.Characters;
using PocketGrail.Domain.Entities.Enums;

public class SkillProficiency : BaseEntity
{
    public int CharacterProficienciesId { get; set; }
    public Skill Skill { get; set; }
    public bool HasExpertise { get; set; }
    public CharacterProficiencies CharacterProficiencies { get; set; } = null!;
}
