namespace PocketGrail.DataAccess.Entities.Proficiencies;

using PocketGrail.DataAccess.Entities.Characters;
using PocketGrail.DataAccess.Entities.Enums;

public class SkillProficiency : BaseEntity
{
    public int CharacterProficienciesId { get; set; }
    public Skill Skill { get; set; }
    public bool HasExpertise { get; set; }
    public CharacterProficiencies CharacterProficiencies { get; set; } = null!;
}
