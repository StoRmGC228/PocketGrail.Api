namespace PocketGrail.Domain.Entities.Characters;

using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Proficiencies;

public class CharacterProficiencies : BaseEntity
{
    public int CharacterId { get; set; }

    public List<SkillProficiency>                 Skills                 { get; set; } = [];
    public List<AdditionalSavingThrowProficiency> AdditionalSavingThrows { get; set; } = [];
    public List<Language>                         Languages              { get; set; } = [];
    public List<Instrument>                       Instruments            { get; set; } = [];
    public List<WeaponProficiency>                Weapons                { get; set; } = [];
    public List<ArmorProficiency>                 Armors                 { get; set; } = [];

    public Character Character { get; set; } = null!;
}
