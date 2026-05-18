namespace PocketGrail.Domain.Entities;

using PocketGrail.Domain.Entities.Proficiencies;

public class Race : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int BaseSpeed { get; set; } = 30;

    public int StrBonus { get; set; }
    public int DexBonus { get; set; }
    public int ConBonus { get; set; }
    public int IntBonus { get; set; }
    public int WisBonus { get; set; }
    public int ChaBonus { get; set; }

    // Extra free bonus points the player distributes (e.g. Half-Elf = 2)
    public int FlexibleBonusPoints { get; set; }
    // Each element is the max value of one slot (e.g. [2,1] = pick one stat +2 and another +1)
    public List<int> FlexBonusSlots { get; set; } = [];

    public List<WeaponProficiency> WeaponGrants { get; set; } = [];
    public List<ArmorProficiency> ArmorGrants { get; set; } = [];
    public List<Language> LanguageGrants { get; set; } = [];
    public List<Instrument> InstrumentGrants { get; set; } = [];

    public List<RaceFeature> Features { get; set; } = [];
}
