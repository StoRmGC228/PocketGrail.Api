namespace PocketGrail.Domain.Entities;

public class Character : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Race { get; set; } = string.Empty;
    public int Level { get; set; } = 1;

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
    public float? ImageCropX { get; set; }
    public float? ImageCropY { get; set; }
    public float? ImageCropWidth { get; set; }
    public float? ImageCropHeight { get; set; }

    // Ability scores
    public int StrScore { get; set; } = 10;
    public int DexScore { get; set; } = 10;
    public int ConScore { get; set; } = 10;
    public int IntScore { get; set; } = 10;
    public int WisScore { get; set; } = 10;
    public int ChaScore { get; set; } = 10;

    // Combat
    public int ArmorClass { get; set; } = 10;
    public int Speed { get; set; } = 30;
    public int XpPoints { get; set; }
    public bool HasInspiration { get; set; }
    public int Exhaustion { get; set; }
    public int DeathSuccesses { get; set; }
    public int DeathFailures { get; set; }

    // Wallet
    public CharacterWallet? Wallet { get; set; }

    // Spellcasting
    public string? SpellAbility { get; set; }

    // Narrative
    public string? Alignment { get; set; }
    public string? BackgroundStory { get; set; }
    public string? Appearance { get; set; }
    public string? Notes { get; set; }

    // Relations
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public int? CampaignId { get; set; }
    public Campaign? Campaign { get; set; }

    // Collections (many-to-many)
    public ICollection<Item> Items { get; set; } = [];
    public ICollection<Spell> Spells { get; set; } = [];
    public ICollection<Feat> Feats { get; set; } = [];
    public ICollection<Feature> Features { get; set; } = [];
    public ICollection<Proficiency> Proficiencies { get; set; } = [];

    // One-to-many
    public ICollection<SpellSlot> SpellSlots { get; set; } = [];
    public ICollection<CharacterClass> Classes { get; set; } = [];
}