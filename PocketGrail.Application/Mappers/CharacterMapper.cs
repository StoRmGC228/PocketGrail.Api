namespace PocketGrail.Application.Mappers;

using PocketGrail.Application.DTOs;
using PocketGrail.Domain.Entities;

public static class CharacterMapper
{
    public static CharacterDto ToDto(Character c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Race = c.Race,
        Classes = c.Classes.Select(ToClassDto).ToList(),
        ClassDisplay = FormatClassDisplay(c.Classes),
        Level = c.Level,
        CurrentHp = c.CurrentHp,
        MaxHp = c.MaxHp,
        ImageUrl = c.ImageUrl,
        OwnerId = c.OwnerId,
        OwnerUsername = c.Owner?.Username ?? string.Empty,
        CampaignId = c.CampaignId,
        CampaignName = c.Campaign?.Name,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt
    };

    public static CharacterDetailDto ToDetailDto(Character c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Race = c.Race,
        Classes = c.Classes.Select(ToClassDto).ToList(),
        ClassDisplay = FormatClassDisplay(c.Classes),
        Level = c.Level,
        ProficiencyBonus = c.ProficiencyBonus,
        CurrentHp = c.CurrentHp,
        MaxHp = c.MaxHp,
        TempHp = c.TempHp,
        ImageUrl = c.ImageUrl,
        ImageCropX = c.ImageCropX,
        ImageCropY = c.ImageCropY,
        ImageCropWidth = c.ImageCropWidth,
        ImageCropHeight = c.ImageCropHeight,
        StrScore = c.StrScore,
        DexScore = c.DexScore,
        ConScore = c.ConScore,
        IntScore = c.IntScore,
        WisScore = c.WisScore,
        ChaScore = c.ChaScore,
        ArmorClass = c.ArmorClass,
        Speed = c.Speed,
        XpPoints = c.XpPoints,
        HasInspiration = c.HasInspiration,
        Exhaustion = c.Exhaustion,
        DeathSuccesses = c.DeathSuccesses,
        DeathFailures = c.DeathFailures,
        CpCoins = c.Wallet?.CpCoins ?? 0,
        SpCoins = c.Wallet?.SpCoins ?? 0,
        EpCoins = c.Wallet?.EpCoins ?? 0,
        GpCoins = c.Wallet?.GpCoins ?? 0,
        PpCoins = c.Wallet?.PpCoins ?? 0,
        SpellAbility = c.SpellAbility,
        Alignment = c.Alignment,
        BackgroundStory = c.BackgroundStory,
        Appearance = c.Appearance,
        Notes = c.Notes,
        OwnerId = c.OwnerId,
        OwnerUsername = c.Owner?.Username ?? string.Empty,
        CampaignId = c.CampaignId,
        CampaignName = c.Campaign?.Name,
        Items = c.Items.Select(i =>
        {
            var j = i.CharacterItems.FirstOrDefault(ci => ci.CharacterId == c.Id);
            return ToItemDto(i, j?.IsEquipped ?? false, j?.IsAttuned ?? false, j?.Quantity ?? 1);
        }).ToList(),
        Spells = c.Spells.Select(s =>
        {
            var j = s.CharacterSpells.FirstOrDefault(cs => cs.CharacterId == c.Id);
            return ToSpellDto(s, j?.Prepared ?? true);
        }).ToList(),
        Feats = c.Feats.Select(f => new FeatDto { Id = f.Id, Name = f.Name, Requirement = f.Requirement, Description = f.Description }).ToList(),
        Features = c.Features.Select(f =>
        {
            var j = f.CharacterFeatures.FirstOrDefault(cf => cf.CharacterId == c.Id);
            return ToFeatureDto(f, j?.IsAutoAdded ?? false);
        }).ToList(),
        Proficiencies = c.Proficiencies.Select(p =>
        {
            var j = p.CharacterProficiencies.FirstOrDefault(cp => cp.CharacterId == c.Id);
            return new ProficiencyDto { Id = p.Id, Name = p.Name, ProficiencyType = p.ProficiencyType, HasExpertise = j?.HasExpertise ?? false, AbilityKey = j?.AbilityKey };
        }).ToList(),
        SpellSlots = c.SpellSlots.Select(s => new SpellSlotDto { Id = s.Id, SlotLevel = s.SlotLevel, TotalSlots = s.TotalSlots, RemainingSlots = s.RemainingSlots }).ToList(),
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt
    };

    public static CharacterClassDto ToClassDto(CharacterClass cc) => new()
    {
        Id = cc.Id,
        ClassName = cc.ClassName,
        ClassLevel = cc.ClassLevel,
        HitDice = cc.HitDice,
        Subclass = cc.Subclass,
        TotalHitDice = cc.TotalHitDice,
        UsedHitDice = cc.UsedHitDice
    };

    public static string FormatClassDisplay(ICollection<CharacterClass> classes) =>
        classes.Count == 0
            ? string.Empty
            : string.Join(" / ", classes.OrderByDescending(c => c.ClassLevel).Select(c => $"{c.ClassName} {c.ClassLevel}"));

    public static ItemDto ToItemDto(Item i, bool equipped, bool attuned, int qty) => new()
    {
        Id = i.Id, Name = i.Name, Description = i.Description, Rarity = i.Rarity, Category = i.Category,
        Weight = i.Weight, Cost = i.Cost, IsWeapon = i.IsWeapon, IsMagical = i.IsMagical,
        AtkMod = i.AtkMod, Damage = i.Damage, DamageType = i.DamageType,
        WeaponProperties = i.WeaponProperties, ChargesInfo = i.ChargesInfo, RechargeType = i.RechargeType,
        Tags = i.Tags, IsEquipped = equipped, IsAttuned = attuned, Quantity = qty
    };

    public static SpellDto ToSpellDto(Spell s, bool prepared) => new()
    {
        Id = s.Id, Name = s.Name, Level = s.Level, School = s.School, Range = s.Range,
        CastingTime = s.CastingTime, Concentration = s.Concentration, IsRitual = s.IsRitual,
        Components = s.Components, Prepared = prepared
    };

    public static FeatureDto ToFeatureDto(Feature f, bool autoAdded) => new()
    {
        Id = f.Id, Name = f.Name, Description = f.Description, FeatureType = f.FeatureType,
        FeatureLevel = f.FeatureLevel, SourceClass = f.SourceClass, SourceRace = f.SourceRace,
        IsAutoAdded = autoAdded
    };
}
