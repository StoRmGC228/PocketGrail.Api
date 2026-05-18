namespace PocketGrail.Application.Mappers;

using PocketGrail.Application.DTOs;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Characters;
using PocketGrail.Domain.Entities.ClassEntities;

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
        UsedHitDice = c.UsedHitDice,
        CurrentHp = c.CurrentHp,
        MaxHp = c.MaxHp,
        TempHp = c.TempHp,
        ImageUrl = c.ImageUrl,
        ImageCropX = c.ImageCropX,
        ImageCropY = c.ImageCropY,
        ImageCropWidth = c.ImageCropWidth,
        ImageCropHeight = c.ImageCropHeight,
        StrScore = c.CharacterStats?.Strength ?? 0,
        DexScore = c.CharacterStats?.Dexterity ?? 0,
        ConScore = c.CharacterStats?.Constitution ?? 0,
        IntScore = c.CharacterStats?.Intelligence ?? 0,
        WisScore = c.CharacterStats?.Wisdom ?? 0,
        ChaScore = c.CharacterStats?.Charisma ?? 0,
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
        Features = c.Features.Select(ToFeatureDto).ToList(),
        SpellSlots = c.SpellSlots.Select(s => new SpellSlotDto { Id = s.Id, SlotLevel = s.SlotLevel, TotalSlots = s.TotalSlots, RemainingSlots = s.RemainingSlots }).ToList(),
        SavingThrows = c.Classes
            .SelectMany(cc => cc.Class.SavingThrows.Select(st => st.Ability))
            .Union(c.Proficiencies?.AdditionalSavingThrows.Select(st => st.Ability) ?? [])
            .Select(a => a.ToString())
            .ToList(),
        SkillProficiencies = c.Proficiencies?.Skills
            .Select(sp => new SkillProficiencyDto { Skill = sp.Skill.ToString(), HasExpertise = sp.HasExpertise })
            .ToList() ?? [],
        Languages = c.Proficiencies?.Languages.Select(l => l.Name).ToList() ?? [],
        Instruments = c.Proficiencies?.Instruments.Select(i => i.Name).ToList() ?? [],
        Weapons = c.Proficiencies?.Weapons.Select(w => w.Name).ToList() ?? [],
        Armors = c.Proficiencies?.Armors.Select(a => a.Name).ToList() ?? [],
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt
    };

    public static CharacterClassDto ToClassDto(CharacterClass cc) => new()
    {
        Id = cc.Id,
        ClassName = cc.Class?.Name ?? string.Empty,
        ClassLevel = cc.ClassLevel,
        HitDice = cc.Class?.HitDice ?? string.Empty,
        Subclass = cc.CharacterSubclass?.Name,
        TotalHitDice = cc.TotalHitDiceCount
    };

    public static string FormatClassDisplay(ICollection<CharacterClass> classes) =>
        classes.Count == 0
            ? string.Empty
            : string.Join(" / ", classes.OrderByDescending(c => c.ClassLevel).Select(c => $"{c.Class?.Name ?? "?"} {c.ClassLevel}"));

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

    public static FeatureDto ToFeatureDto(Feature f) => new()
    {
        Id = f.Id, Name = f.Name, Description = f.Description
    };

    public static SubclassDto ToSubclassDto(Subclass s) => new()
    {
        Id = s.Id, Name = s.Name, ShortDescription = s.ShortDescription, ClassId = s.ClassId
    };

    public static ClassDto ToClassInfoDto(Class c) => new()
    {
        Id              = c.Id,
        Name            = c.Name,
        HitDice         = c.HitDice,
        SpellAbility    = c.SpellAbility,
        SkillChoiceCount = c.SkillChoiceCount,
        Subclasses      = c.Subclasses?.Select(ToSubclassDto).ToList() ?? []
    };

    public static RaceDto ToRaceDto(Race r) => new()
    {
        Id                  = r.Id,
        Name                = r.Name,
        BaseSpeed           = r.BaseSpeed,
        StrBonus            = r.StrBonus,
        DexBonus            = r.DexBonus,
        ConBonus            = r.ConBonus,
        IntBonus            = r.IntBonus,
        WisBonus            = r.WisBonus,
        ChaBonus            = r.ChaBonus,
        FlexibleBonusPoints = r.FlexibleBonusPoints,
        WeaponGrants        = r.WeaponGrants.Select(w => w.Name).ToList(),
        ArmorGrants         = r.ArmorGrants.Select(a => a.Name).ToList(),
        LanguageGrants      = r.LanguageGrants.Select(l => l.Name).ToList(),
        InstrumentGrants    = r.InstrumentGrants.Select(i => i.Name).ToList(),
        Features            = r.Features.Select(f => new RaceFeatureDto { Id = f.Id, Name = f.Name, Description = f.Description }).ToList()
    };
}
