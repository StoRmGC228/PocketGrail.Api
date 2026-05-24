namespace PocketGrail.Application.Mappers;

using DA = PocketGrail.DataAccess.Entities;
using DAChar = PocketGrail.DataAccess.Entities.Characters;
using DAClass = PocketGrail.DataAccess.Entities.ClassEntities;
using DAProf = PocketGrail.DataAccess.Entities.Proficiencies;
using Domain = PocketGrail.Domain.Aggregates;
using DomainEnum = PocketGrail.Domain.Enums;
using DomainVal = PocketGrail.Domain.ValueObjects;
using DomainSup = PocketGrail.Domain.SupportingTypes;

public static class CharacterDomainMapper
{
    public static Domain.Character ToDomain(
        DAChar.Character entity,
        DAClass.Class? catalogClass = null)
    {
        var stats = entity.CharacterStats is not null
            ? DomainVal.CharacterStats.Create(
                entity.CharacterStats.Strength,     entity.CharacterStats.Dexterity,
                entity.CharacterStats.Constitution, entity.CharacterStats.Intelligence,
                entity.CharacterStats.Wisdom,       entity.CharacterStats.Charisma)
            : null;

        var wallet = entity.Wallet is not null
            ? DomainVal.CharacterWallet.Create(
                entity.Wallet.CpCoins, entity.Wallet.SpCoins, entity.Wallet.EpCoins,
                entity.Wallet.GpCoins, entity.Wallet.PpCoins)
            : DomainVal.CharacterWallet.Empty;

        var classes = entity.Classes
            .Select(cc => MapClassToDomain(cc, catalogClass?.Id == cc.ClassId ? catalogClass : null))
            .ToList();

        var items = entity.Items
            .Select(i =>
            {
                var j = i.CharacterItems.FirstOrDefault(ci => ci.CharacterId == entity.Id);
                return new DomainSup.OwnedItem(i.Id, j?.IsEquipped ?? false, j?.IsAttuned ?? false, j?.Quantity ?? 1);
            })
            .ToList();

        var spells = entity.Spells
            .Select(s =>
            {
                var j = s.CharacterSpells.FirstOrDefault(cs => cs.CharacterId == entity.Id);
                return new DomainSup.OwnedSpell(s.Id, s.Level, j?.Prepared ?? true);
            })
            .ToList();

        var feats      = entity.Feats.Select(f => new DomainSup.OwnedFeat(f.Id)).ToList();
        var spellSlots = entity.SpellSlots.Select(s => new DomainSup.OwnedSpellSlot(s.SlotLevel, s.TotalSlots, s.RemainingSlots)).ToList();
        var features   = entity.Features.Select(f => new DomainSup.CharacterFeature(f.Id, f.Name, f.Description)).ToList();
        var profs      = MapProficienciesToDomain(entity.Proficiencies);

        return Domain.Character.Reconstitute(
            entity.Id, entity.OwnerId, entity.CampaignId,
            entity.Name, entity.Race, entity.Level, entity.XpPoints,
            entity.Alignment, entity.BackgroundStory, entity.Appearance, entity.Notes, entity.ImageUrl,
            entity.CurrentHp, entity.MaxHp, entity.TempHp, entity.ArmorClass, entity.Speed,
            entity.HasInspiration, entity.Exhaustion, entity.DeathSuccesses, entity.DeathFailures,
            entity.TotalHitDiceCount, entity.UsedHitDice,
            stats, wallet, classes, spells, items, feats, spellSlots, features, profs);
    }

    public static void UpdatePersistence(Domain.Character domain, DAChar.Character entity)
    {
        var now = DateTime.UtcNow;

        entity.Name            = domain.Name;
        entity.Race            = domain.RaceName;
        entity.Level           = domain.Level;
        entity.CurrentHp       = domain.CurrentHp;
        entity.MaxHp           = domain.MaxHp;
        entity.TempHp          = domain.TempHp;
        entity.ArmorClass      = domain.ArmorClass;
        entity.Speed           = domain.Speed;
        entity.XpPoints        = domain.XpPoints;
        entity.HasInspiration  = domain.HasInspiration;
        entity.Exhaustion      = domain.Exhaustion;
        entity.DeathSuccesses  = domain.DeathSuccesses;
        entity.DeathFailures   = domain.DeathFailures;
        entity.TotalHitDiceCount = domain.TotalHitDiceCount;
        entity.UsedHitDice     = domain.UsedHitDice;
        entity.ImageUrl        = domain.ImageUrl;
        entity.Alignment       = domain.Alignment;
        entity.BackgroundStory = domain.BackgroundStory;
        entity.Appearance      = domain.Appearance;
        entity.Notes           = domain.Notes;
        entity.CampaignId      = domain.CampaignId;
        entity.UpdatedAt       = now;

        if (domain.Stats is not null && entity.CharacterStats is not null)
        {
            entity.CharacterStats.Strength     = domain.Stats.Strength;
            entity.CharacterStats.Dexterity    = domain.Stats.Dexterity;
            entity.CharacterStats.Constitution = domain.Stats.Constitution;
            entity.CharacterStats.Intelligence = domain.Stats.Intelligence;
            entity.CharacterStats.Wisdom       = domain.Stats.Wisdom;
            entity.CharacterStats.Charisma     = domain.Stats.Charisma;
            entity.CharacterStats.UpdatedAt    = now;
        }

        if (entity.Wallet is not null)
        {
            entity.Wallet.CpCoins    = domain.Wallet.Cp;
            entity.Wallet.SpCoins    = domain.Wallet.Sp;
            entity.Wallet.EpCoins    = domain.Wallet.Ep;
            entity.Wallet.GpCoins    = domain.Wallet.Gp;
            entity.Wallet.PpCoins    = domain.Wallet.Pp;
            entity.Wallet.UpdatedAt  = now;
        }

        foreach (var domainClass in domain.Classes)
        {
            var entityClass = entity.Classes.FirstOrDefault(cc => cc.Id == domainClass.Id);
            if (entityClass is not null)
            {
                entityClass.ClassLevel          = domainClass.ClassLevel;
                entityClass.TotalHitDiceCount   = domainClass.ClassLevel;
                entityClass.CharacterSubclassId = domainClass.SubclassId;
                entityClass.UpdatedAt           = now;
            }
        }

        foreach (var domainSlot in domain.SpellSlots)
        {
            var entitySlot = entity.SpellSlots.FirstOrDefault(s => s.SlotLevel == domainSlot.SlotLevel);
            if (entitySlot is not null)
            {
                entitySlot.TotalSlots     = domainSlot.TotalSlots;
                entitySlot.RemainingSlots = domainSlot.RemainingSlots;
                entitySlot.UpdatedAt      = now;
            }
            else
            {
                entity.SpellSlots.Add(new DA.SpellSlot
                {
                    CharacterId    = entity.Id,
                    SlotLevel      = domainSlot.SlotLevel,
                    TotalSlots     = domainSlot.TotalSlots,
                    RemainingSlots = domainSlot.RemainingSlots,
                    CreatedAt      = now,
                    UpdatedAt      = now
                });
            }
        }

        var keptFeatureIds = domain.Features.Where(f => f.PersistenceId > 0).Select(f => f.PersistenceId).ToHashSet();
        foreach (var toRemove in entity.Features.Where(f => !keptFeatureIds.Contains(f.Id)).ToList())
            entity.Features.Remove(toRemove);

        foreach (var df in domain.Features.Where(f => f.PersistenceId == 0))
            entity.Features.Add(new DA.Feature { Name = df.Name, Description = df.Description, CreatedAt = now, UpdatedAt = now });

        SyncItemJunctions(domain, entity);
        SyncSpellJunctions(domain, entity);

        if (entity.Proficiencies is not null)
            SyncProficiencies(domain, entity.Proficiencies, now);
    }

    // ── Private helpers ─────────────────────────────────────────────────────────

    private static DomainSup.CharacterClassData MapClassToDomain(DAChar.CharacterClass cc, DAClass.Class? cls)
    {
        var hitDiceValue = ParseHitDice(cc.Class?.HitDice ?? "d8");

        var featureTemplates = cls?.ClassFeatures
            .Select(cf => new DomainSup.ClassFeatureTemplate(cf.GainingLevel, cf.Name, cf.Description))
            .ToList() ?? new List<DomainSup.ClassFeatureTemplate>();

        var slotTemplates = cls?.SpellSlotTemplates
            .Select(s => new DomainSup.SpellSlotTemplate(s.ClassLevel, s.SpellSlotLevel, s.TotalSlots))
            .ToList() ?? new List<DomainSup.SpellSlotTemplate>();

        var savingThrows = cls?.SavingThrows
            .Select(st => st.Ability.ToString())
            .ToList() ?? new List<string>();

        var skillChoices = cls?.AvailableSkillChoices
            .Select(sc => sc.Skill.ToString())
            .ToList() ?? new List<string>();

        return new DomainSup.CharacterClassData(
            id:                    cc.Id,
            classId:               cc.ClassId,
            className:             cc.Class?.Name ?? string.Empty,
            classLevel:            cc.ClassLevel,
            hitDiceValue:          hitDiceValue,
            subclassId:            cc.CharacterSubclassId,
            subclassName:          cc.CharacterSubclass?.Name,
            skillChoiceCount:      cls?.SkillChoiceCount ?? 0,
            availableSkillChoices: skillChoices,
            availableSavingThrows: savingThrows,
            multiclassProficiencies: new List<string>(),
            allFeatureTemplates:   featureTemplates,
            allSpellSlotTemplates: slotTemplates);
    }

    private static int ParseHitDice(string hitDice)
    {
        if (hitDice.StartsWith("d", StringComparison.OrdinalIgnoreCase)
            && int.TryParse(hitDice[1..], out var val))
            return val;
        return 8;
    }

    private static DomainSup.CharacterProficiencySet MapProficienciesToDomain(DAChar.CharacterProficiencies? profs)
    {
        if (profs is null) return new DomainSup.CharacterProficiencySet();

        var skills = profs.Skills
            .Where(sp => Enum.TryParse<DomainEnum.Skill>(sp.Skill.ToString(), out _))
            .Select(sp => (Enum.Parse<DomainEnum.Skill>(sp.Skill.ToString()), sp.HasExpertise));

        var savingThrows = profs.AdditionalSavingThrows
            .Where(st => Enum.TryParse<DomainEnum.Ability>(st.Ability.ToString(), out _))
            .Select(st => Enum.Parse<DomainEnum.Ability>(st.Ability.ToString()));

        return DomainSup.CharacterProficiencySet.Reconstitute(
            skills,
            profs.Weapons.Select(w => w.Name),
            profs.Armors.Select(a => a.Name),
            profs.Languages.Select(l => l.Name),
            profs.Instruments.Select(i => i.Name),
            savingThrows);
    }

    private static void SyncItemJunctions(Domain.Character domain, DAChar.Character entity)
    {
        foreach (var domainItem in domain.Items)
        {
            var catalogItem = entity.Items.FirstOrDefault(i => i.Id == domainItem.ItemId);
            if (catalogItem is null) continue;
            var junction = catalogItem.CharacterItems.FirstOrDefault(ci => ci.CharacterId == entity.Id);
            if (junction is null) continue;
            junction.IsEquipped = domainItem.IsEquipped;
            junction.IsAttuned  = domainItem.IsAttuned;
            junction.Quantity   = domainItem.Quantity;
        }
    }

    private static void SyncSpellJunctions(Domain.Character domain, DAChar.Character entity)
    {
        foreach (var domainSpell in domain.Spells)
        {
            var catalogSpell = entity.Spells.FirstOrDefault(s => s.Id == domainSpell.SpellId);
            if (catalogSpell is null) continue;
            var junction = catalogSpell.CharacterSpells.FirstOrDefault(cs => cs.CharacterId == entity.Id);
            if (junction is null) continue;
            junction.Prepared = domainSpell.IsPrepared;
        }
    }

    private static void SyncProficiencies(Domain.Character domain, DAChar.CharacterProficiencies profs, DateTime now)
    {
        var domainSkills = domain.Proficiencies.Skills;

        foreach (var sp in profs.Skills.ToList())
        {
            if (!Enum.TryParse<DomainEnum.Skill>(sp.Skill.ToString(), out var domainSkill)
                || !domainSkills.Any(s => s.Skill == domainSkill))
                profs.Skills.Remove(sp);
        }
        foreach (var (skill, expertise) in domainSkills)
        {
            var daSkill = Enum.Parse<DataAccess.Entities.Enums.Skill>(skill.ToString());
            if (!profs.Skills.Any(s => s.Skill == daSkill))
                profs.Skills.Add(new DAProf.SkillProficiency
                {
                    Skill = daSkill, HasExpertise = expertise,
                    CharacterProficienciesId = profs.Id, CreatedAt = now, UpdatedAt = now
                });
        }

        foreach (var st in profs.AdditionalSavingThrows.ToList())
        {
            if (!Enum.TryParse<DomainEnum.Ability>(st.Ability.ToString(), out var domainAbility)
                || !domain.Proficiencies.SavingThrows.Contains(domainAbility))
                profs.AdditionalSavingThrows.Remove(st);
        }
        foreach (var ability in domain.Proficiencies.SavingThrows)
        {
            var daAbility = Enum.Parse<DataAccess.Entities.Enums.Ability>(ability.ToString());
            if (!profs.AdditionalSavingThrows.Any(s => s.Ability == daAbility))
                profs.AdditionalSavingThrows.Add(new DAProf.AdditionalSavingThrowProficiency
                {
                    Ability = daAbility,
                    CharacterProficienciesId = profs.Id, CreatedAt = now, UpdatedAt = now
                });
        }

        SyncManyToManyProficiency(profs.Weapons, domain.Proficiencies.Weapons,
            n => new DAProf.WeaponProficiency { Name = n, CreatedAt = now, UpdatedAt = now });

        SyncManyToManyProficiency(profs.Armors, domain.Proficiencies.Armors,
            n => new DAProf.ArmorProficiency { Name = n, CreatedAt = now, UpdatedAt = now });

        SyncManyToManyProficiency(profs.Languages, domain.Proficiencies.Languages,
            n => new DA.Language { Name = n, CreatedAt = now, UpdatedAt = now });

        SyncManyToManyProficiency(profs.Instruments, domain.Proficiencies.Instruments,
            n => new DA.Instrument { Name = n, CreatedAt = now, UpdatedAt = now });

        profs.UpdatedAt = now;
    }

    private static void SyncManyToManyProficiency<T>(
        List<T> entityList,
        IReadOnlyList<string> domainNames,
        Func<string, T> factory) where T : DA.BaseEntity
    {
        foreach (var e in entityList.ToList())
        {
            var name = (e as dynamic)?.Name as string;
            if (name is null || !domainNames.Any(n => n.Equals(name, StringComparison.OrdinalIgnoreCase)))
                entityList.Remove(e);
        }
        foreach (var name in domainNames)
        {
            var existing = entityList.Cast<dynamic>().Any(e => ((string)e.Name).Equals(name, StringComparison.OrdinalIgnoreCase));
            if (!existing)
                entityList.Add(factory(name));
        }
    }
}
