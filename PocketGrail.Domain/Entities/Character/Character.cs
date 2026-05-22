namespace PocketGrail.Domain.Entities.Characters;

using PocketGrail.Domain.Entities;

public class Character : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Race { get; set; } = string.Empty;
    public int Level { get; set; } = 1;

    public int TotalHitDiceCount { get; set; }
    public int UsedHitDice { get; set; }

    public int ProficiencyBonus => Level switch
    {
        >= 15 => 5,
        >= 10 => 4,
        >= 5 => 3,
        _ => 2
    };

    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public int TempHp { get; set; }
    public string? ImageUrl { get; set; }

    public CharacterStats? CharacterStats { get; set; }

    public int ArmorClass { get; set; } = 10;
    public int Speed { get; set; } = 30;
    public int XpPoints { get; set; }
    public bool HasInspiration { get; set; }
    public int Exhaustion { get; set; }
    public int DeathSuccesses { get; set; }
    public int DeathFailures { get; set; }

    public CharacterWallet? Wallet { get; set; }

    public string? Alignment { get; set; }
    public string? BackgroundStory { get; set; }
    public string? Appearance { get; set; }
    public string? Notes { get; set; }

    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public int? CampaignId { get; set; }
    public Campaign? Campaign { get; set; }

    public ICollection<Item> Items { get; set; } = [];
    public ICollection<Spell> Spells { get; set; } = [];
    public ICollection<Feat> Feats { get; set; } = [];
    public ICollection<Feature> Features { get; set; } = [];

    public ICollection<SpellSlot> SpellSlots { get; set; } = [];
    public ICollection<CharacterClass> Classes { get; set; } = [];

    public CharacterProficiencies? Proficiencies { get; set; }
}
