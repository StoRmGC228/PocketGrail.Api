namespace PocketGrail.Domain.Entities.Proficiencies;

using PocketGrail.Domain.Entities.Characters;
using PocketGrail.Domain.Entities.Enums;

public class AdditionalSavingThrowProficiency : BaseEntity
{
    public int CharacterProficienciesId { get; set; }
    public Ability Ability { get; set; }
    public CharacterProficiencies CharacterProficiencies { get; set; } = null!;
}
