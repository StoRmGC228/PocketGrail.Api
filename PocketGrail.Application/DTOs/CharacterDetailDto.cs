namespace PocketGrail.Application.DTOs;

public sealed class CharacterDetailDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Race { get; init; } = string.Empty;
    public IReadOnlyList<CharacterClassDto> Classes { get; init; } = [];
    public string ClassDisplay { get; init; } = string.Empty;
    public int Level { get; init; }
    public int ProficiencyBonus { get; init; }
    public int CurrentHp { get; init; }
    public int MaxHp { get; init; }
    public int TempHp { get; init; }
    public string? ImageUrl { get; init; }
    public float? ImageCropX { get; init; }
    public float? ImageCropY { get; init; }
    public float? ImageCropWidth { get; init; }
    public float? ImageCropHeight { get; init; }

    // Ability scores
    public int StrScore { get; init; }
    public int DexScore { get; init; }
    public int ConScore { get; init; }
    public int IntScore { get; init; }
    public int WisScore { get; init; }
    public int ChaScore { get; init; }

    // Combat
    public int ArmorClass { get; init; }
    public int Speed { get; init; }
    public int XpPoints { get; init; }
    public bool HasInspiration { get; init; }
    public int Exhaustion { get; init; }
    public int DeathSuccesses { get; init; }
    public int DeathFailures { get; init; }

    // Wallet
    public int CpCoins { get; init; }
    public int SpCoins { get; init; }
    public int EpCoins { get; init; }
    public int GpCoins { get; init; }
    public int PpCoins { get; init; }

    // Spellcasting
    public string? SpellAbility { get; init; }

    // Narrative
    public string? Alignment { get; init; }
    public string? BackgroundStory { get; init; }
    public string? Appearance { get; init; }
    public string? Notes { get; init; }

    // Owner / Campaign
    public int OwnerId { get; init; }
    public string OwnerUsername { get; init; } = string.Empty;
    public int? CampaignId { get; init; }
    public string? CampaignName { get; init; }

    // Collections
    public IReadOnlyList<ItemDto> Items { get; init; } = [];
    public IReadOnlyList<SpellDto> Spells { get; init; } = [];
    public IReadOnlyList<FeatDto> Feats { get; init; } = [];
    public IReadOnlyList<FeatureDto> Features { get; init; } = [];
    public IReadOnlyList<ProficiencyDto> Proficiencies { get; init; } = [];
    public IReadOnlyList<SpellSlotDto> SpellSlots { get; init; } = [];

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
