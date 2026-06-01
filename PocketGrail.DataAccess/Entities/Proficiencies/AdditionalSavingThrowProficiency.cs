namespace PocketGrail.DataAccess.Entities.Proficiencies;

using PocketGrail.DataAccess.Entities.Characters;
using PocketGrail.DataAccess.Entities.Enums;

public class AdditionalSavingThrowProficiency : BaseEntity
{
    public int CharacterProficienciesId { get; set; }
    public Ability Ability { get; set; }
    public CharacterProficiencies CharacterProficiencies { get; set; } = null!;
}
