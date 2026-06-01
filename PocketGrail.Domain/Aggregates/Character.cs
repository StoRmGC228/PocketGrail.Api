namespace PocketGrail.Domain.Aggregates;

using PocketGrail.Domain.Enums;
using PocketGrail.Domain.Exceptions;
using PocketGrail.Domain.SupportingTypes;
using PocketGrail.Domain.ValueObjects;

public sealed class Character
{
    // ── Identity & Ownership ────────────────────────────────────────────────────
    public int Id { get; private set; }
    public int OwnerId { get; private set; }
    public int? CampaignId { get; private set; }

    // ── Basic Info ──────────────────────────────────────────────────────────────
    public string Name { get; private set; } = string.Empty;
    public string RaceName { get; private set; } = string.Empty;
    public int Level { get; private set; }
    public int XpPoints { get; private set; }
    public string? Alignment { get; private set; }
    public string? BackgroundStory { get; private set; }
    public string? Appearance { get; private set; }
    public string? Notes { get; private set; }
    public string? ImageUrl { get; private set; }

    // ── Vitals ──────────────────────────────────────────────────────────────────
    public int CurrentHp { get; private set; }
    public int MaxHp { get; private set; }
    public int TempHp { get; private set; }
    public int ArmorClass { get; private set; } = 10;
    public int Speed { get; private set; } = 30;
    public bool HasInspiration { get; private set; }
    public int Exhaustion { get; private set; }
    public int DeathSuccesses { get; private set; }
    public int DeathFailures { get; private set; }
    public int TotalHitDiceCount { get; private set; }
    public int UsedHitDice { get; private set; }

    // ── Value Objects ────────────────────────────────────────────────────────────
    public CharacterStats? Stats { get; private set; }
    public CharacterWallet Wallet { get; private set; } = CharacterWallet.Empty;

    // ── Collections ─────────────────────────────────────────────────────────────
    private readonly List<CharacterClassData> _classes;
    private readonly List<OwnedSpell> _spells;
    private readonly List<OwnedItem> _items;
    private readonly List<OwnedFeat> _feats;
    private readonly List<OwnedSpellSlot> _spellSlots;
    private readonly List<CharacterFeature> _features;
    private readonly CharacterProficiencySet _proficiencies;

    public IReadOnlyList<CharacterClassData> Classes     => _classes.AsReadOnly();
    public IReadOnlyList<OwnedSpell>         Spells      => _spells.AsReadOnly();
    public IReadOnlyList<OwnedItem>          Items       => _items.AsReadOnly();
    public IReadOnlyList<OwnedFeat>          Feats       => _feats.AsReadOnly();
    public IReadOnlyList<OwnedSpellSlot>     SpellSlots  => _spellSlots.AsReadOnly();
    public IReadOnlyList<CharacterFeature>   Features    => _features.AsReadOnly();
    public CharacterProficiencySet           Proficiencies => _proficiencies;

    // ── Computed ─────────────────────────────────────────────────────────────────
    public int ProficiencyBonus => Level switch
    {
        >= 17 => 6,
        >= 13 => 5,
        >= 9  => 4,
        >= 5  => 3,
        _     => 2
    };

    // ── Constructor ─────────────────────────────────────────────────────────────
    private Character(
        int id,
        int ownerId,
        int? campaignId,
        string name,
        string raceName,
        int level,
        int xpPoints,
        string? alignment,
        string? backgroundStory,
        string? appearance,
        string? notes,
        string? imageUrl,
        int currentHp,
        int maxHp,
        int tempHp,
        int armorClass,
        int speed,
        bool hasInspiration,
        int exhaustion,
        int deathSuccesses,
        int deathFailures,
        int totalHitDiceCount,
        int usedHitDice,
        CharacterStats? stats,
        CharacterWallet wallet,
        List<CharacterClassData> classes,
        List<OwnedSpell> spells,
        List<OwnedItem> items,
        List<OwnedFeat> feats,
        List<OwnedSpellSlot> spellSlots,
        List<CharacterFeature> features,
        CharacterProficiencySet proficiencies)
    {
        Id = id;
        OwnerId = ownerId;
        CampaignId = campaignId;
        Name = name;
        RaceName = raceName;
        Level = level;
        XpPoints = xpPoints;
        Alignment = alignment;
        BackgroundStory = backgroundStory;
        Appearance = appearance;
        Notes = notes;
        ImageUrl = imageUrl;
        CurrentHp = currentHp;
        MaxHp = maxHp;
        TempHp = tempHp;
        ArmorClass = armorClass;
        Speed = speed;
        HasInspiration = hasInspiration;
        Exhaustion = exhaustion;
        DeathSuccesses = deathSuccesses;
        DeathFailures = deathFailures;
        TotalHitDiceCount = totalHitDiceCount;
        UsedHitDice = usedHitDice;
        Stats = stats;
        Wallet = wallet;
        _classes = classes;
        _spells = spells;
        _items = items;
        _feats = feats;
        _spellSlots = spellSlots;
        _features = features;
        _proficiencies = proficiencies;
    }

    // ── Factory: reconstitute from persistence ─────────────────────────────────
    public static Character Reconstitute(
        int id,
        int ownerId,
        int? campaignId,
        string name,
        string raceName,
        int level,
        int xpPoints,
        string? alignment,
        string? backgroundStory,
        string? appearance,
        string? notes,
        string? imageUrl,
        int currentHp,
        int maxHp,
        int tempHp,
        int armorClass,
        int speed,
        bool hasInspiration,
        int exhaustion,
        int deathSuccesses,
        int deathFailures,
        int totalHitDiceCount,
        int usedHitDice,
        CharacterStats? stats,
        CharacterWallet wallet,
        List<CharacterClassData> classes,
        List<OwnedSpell> spells,
        List<OwnedItem> items,
        List<OwnedFeat> feats,
        List<OwnedSpellSlot> spellSlots,
        List<CharacterFeature> features,
        CharacterProficiencySet proficiencies) =>
        new(id, ownerId, campaignId, name, raceName, level, xpPoints,
            alignment, backgroundStory, appearance, notes, imageUrl,
            currentHp, maxHp, tempHp, armorClass, speed,
            hasInspiration, exhaustion, deathSuccesses, deathFailures,
            totalHitDiceCount, usedHitDice,
            stats, wallet, classes, spells, items, feats, spellSlots, features, proficiencies);

    // ── Factory: create new character ────────────────────────────────────────────
    public static Character Create(string name, int ownerId, int? campaignId, string raceName, int speed) =>
        new(0, ownerId, campaignId, name, raceName,
            level: 0, xpPoints: 0,
            alignment: null, backgroundStory: null, appearance: null, notes: null, imageUrl: null,
            currentHp: 0, maxHp: 0, tempHp: 0, armorClass: 10, speed,
            hasInspiration: false, exhaustion: 0, deathSuccesses: 0, deathFailures: 0,
            totalHitDiceCount: 0, usedHitDice: 0,
            stats: null, wallet: CharacterWallet.Empty,
            classes: new(), spells: new(), items: new(), feats: new(),
            spellSlots: new(), features: new(), proficiencies: new());

    // ── Vitals ───────────────────────────────────────────────────────────────────

    public void TakeDamage(int amount)
    {
        if (amount < 0) throw new DomainException("Damage amount cannot be negative.");
        var afterTemp = TempHp - amount;
        if (afterTemp >= 0)
        {
            TempHp = afterTemp;
            return;
        }
        TempHp = 0;
        CurrentHp = Math.Max(-MaxHp, CurrentHp + afterTemp);
    }

    public void Heal(int amount)
    {
        if (amount < 0) throw new DomainException("Healing amount cannot be negative.");
        CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
    }

    public void SetTempHp(int amount)
    {
        if (amount < 0) throw new DomainException("Temporary HP cannot be negative.");
        TempHp = amount;
    }

    public void UpdateMaxHp(int newMaxHp)
    {
        if (newMaxHp < 1) throw new DomainException("Max HP must be at least 1.");
        if (CurrentHp > newMaxHp) CurrentHp = newMaxHp;
        MaxHp = newMaxHp;
    }

    public void UpdateArmorClass(int ac)
    {
        if (ac < 0) throw new DomainException("Armor class cannot be negative.");
        ArmorClass = ac;
    }

    public void UpdateSpeed(int speed)
    {
        if (speed < 0) throw new DomainException("Speed cannot be negative.");
        Speed = speed;
    }

    public void UpdateVitals(int? currentHp, int? maxHp, int? tempHp, int? xpPoints,
        bool? hasInspiration, int? exhaustion, int? deathSuccesses, int? deathFailures)
    {
        if (maxHp.HasValue)
        {
            if (maxHp.Value < 0) throw new DomainException("Max HP cannot be negative.");
            MaxHp = maxHp.Value;
            if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        }
        if (currentHp.HasValue) CurrentHp = currentHp.Value;
        if (tempHp.HasValue)
        {
            if (tempHp.Value < 0) throw new DomainException("Temporary HP cannot be negative.");
            TempHp = tempHp.Value;
        }
        if (xpPoints.HasValue) XpPoints = xpPoints.Value;
        if (hasInspiration.HasValue) HasInspiration = hasInspiration.Value;
        if (exhaustion.HasValue) Exhaustion = exhaustion.Value;
        if (deathSuccesses.HasValue) DeathSuccesses = deathSuccesses.Value;
        if (deathFailures.HasValue) DeathFailures = deathFailures.Value;
    }

    // ── Stats ────────────────────────────────────────────────────────────────────

    public void UpdateStats(CharacterStats stats) => Stats = stats;

    public void UpdateStatsAndMisc(CharacterStats? stats, int? armorClass, int? speed, string? alignment)
    {
        if (stats is not null) Stats = stats;
        if (armorClass.HasValue) UpdateArmorClass(armorClass.Value);
        if (speed.HasValue) UpdateSpeed(speed.Value);
        if (alignment is not null) Alignment = alignment;
    }

    // ── Basic Info ───────────────────────────────────────────────────────────────

    public void UpdateInfo(string? name, string? raceName, int? campaignId,
        string? alignment, string? backgroundStory, string? appearance, string? notes)
    {
        if (name is not null) Name = name;
        if (raceName is not null) RaceName = raceName;
        if (campaignId.HasValue) CampaignId = campaignId.Value;
        if (alignment is not null) Alignment = alignment;
        if (backgroundStory is not null) BackgroundStory = backgroundStory;
        if (appearance is not null) Appearance = appearance;
        if (notes is not null) Notes = notes;
    }

    // ── Image ────────────────────────────────────────────────────────────────────

    public void SetImageUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new DomainException("Image URL cannot be empty.");
        ImageUrl = url;
    }

    // ── Classes ──────────────────────────────────────────────────────────────────

    public void AddClass(CharacterClassData classData, IReadOnlyList<string> chosenSkills)
    {
        if (_classes.Any(c => c.ClassId == classData.ClassId))
            throw new DomainException($"Character already has a level in {classData.ClassName}.");

        if (chosenSkills.Count != classData.SkillChoiceCount)
            throw new DomainException(
                $"Must choose exactly {classData.SkillChoiceCount} skills for {classData.ClassName}. Got {chosenSkills.Count}.");

        foreach (var skill in chosenSkills)
        {
            if (!classData.AvailableSkillChoices.Any(s => s.Equals(skill, StringComparison.OrdinalIgnoreCase)))
                throw new DomainException($"Skill '{skill}' is not available for {classData.ClassName}.");
            if (Enum.TryParse<Skill>(skill, true, out var skillEnum))
                _proficiencies.AddSkill(skillEnum, false);
        }

        var isFirstClass = _classes.Count == 0;

        if (isFirstClass)
        {
            foreach (var st in classData.AvailableSavingThrows)
            {
                if (Enum.TryParse<Ability>(st, true, out var ability))
                    _proficiencies.AddSavingThrow(ability);
            }
        }

        foreach (var prof in classData.MulticlassProficiencies)
            _proficiencies.AddWeapon(prof);

        foreach (var template in classData.AllFeatureTemplates
            .Where(f => f.GainingLevel == 1 && f.Name != "Starting Proficiencies"))
        {
            if (!_features.Any(f => f.Name == template.Name))
                _features.Add(new CharacterFeature(0, template.Name, template.Description));
        }

        foreach (var slot in classData.AllSpellSlotTemplates.Where(s => s.ClassLevel == 1 && s.TotalSlots > 0))
        {
            if (!_spellSlots.Any(s => s.SlotLevel == slot.SlotLevel))
                _spellSlots.Add(new OwnedSpellSlot(slot.SlotLevel, slot.TotalSlots, slot.TotalSlots));
        }

        _classes.Add(classData);
        Level++;
        TotalHitDiceCount++;
    }

    public void LevelUp(int characterClassId, AsiOrFeatChoice? choice)
    {
        if (Level >= 20) throw new DomainException("Character has reached the maximum level of 20.");

        var classData = _classes.FirstOrDefault(c => c.Id == characterClassId)
            ?? throw new DomainException("Class entry not found on this character.");

        var newClassLevel = classData.ClassLevel + 1;

        var isAsiLevel = classData.AllFeatureTemplates
            .Any(f => f.GainingLevel == newClassLevel && f.Name == "Ability Score Improvement");

        if (isAsiLevel && choice is null)
            throw new DomainException("An ability score improvement choice is required for this level.");

        if (!isAsiLevel && choice is not null)
            throw new DomainException("No ability score improvement is available at this level.");

        if (choice is not null)
        {
            if (choice.IsAsi)
                ApplyAsi(choice);
            else
            {
                var featId = choice.FeatId ?? throw new DomainException("FeatId is required for a feat choice.");
                AddFeat(featId);
            }
        }

        foreach (var template in classData.AllFeatureTemplates
            .Where(f => f.GainingLevel == newClassLevel && f.Name != "Starting Proficiencies"))
        {
            if (!_features.Any(f => f.Name == template.Name))
                _features.Add(new CharacterFeature(0, template.Name, template.Description));
        }

        foreach (var slot in classData.AllSpellSlotTemplates.Where(s => s.ClassLevel == newClassLevel && s.TotalSlots > 0))
        {
            var existing = _spellSlots.FirstOrDefault(s => s.SlotLevel == slot.SlotLevel);
            if (existing is null)
                _spellSlots.Add(new OwnedSpellSlot(slot.SlotLevel, slot.TotalSlots, slot.TotalSlots));
            else if (slot.TotalSlots > existing.TotalSlots)
                existing.UpdateTotalSlots(slot.TotalSlots);
        }

        var conMod = Stats?.ConstitutionModifier ?? 0;
        MaxHp += classData.HitDiceValue / 2 + 1 + conMod;
        if (MaxHp < 1) MaxHp = 1;

        classData.IncrementLevel();
        Level++;
        TotalHitDiceCount++;
    }

    public void SetSubclass(int characterClassId, int subclassId, string subclassName,
        IReadOnlyList<ClassFeatureTemplate> subclassFeatures)
    {
        var classData = _classes.FirstOrDefault(c => c.Id == characterClassId)
            ?? throw new DomainException("Class entry not found on this character.");

        if (classData.SubclassId.HasValue)
            throw new DomainException($"A subclass is already set for {classData.ClassName}.");

        classData.SetSubclass(subclassId, subclassName);

        foreach (var template in subclassFeatures.Where(f => f.GainingLevel <= classData.ClassLevel))
        {
            if (!_features.Any(f => f.Name == template.Name))
                _features.Add(new CharacterFeature(0, template.Name, template.Description));
        }
    }

    public void RemoveClass(int characterClassId)
    {
        if (_classes.Count <= 1)
            throw new DomainException("Cannot remove the last class from a character.");

        var classData = _classes.FirstOrDefault(c => c.Id == characterClassId)
            ?? throw new DomainException("Class entry not found on this character.");

        Level -= classData.ClassLevel;
        TotalHitDiceCount -= classData.ClassLevel;
        _classes.Remove(classData);
    }

    public void UpdateUsedHitDice(int characterClassId, int usedHitDice)
    {
        var classData = _classes.FirstOrDefault(c => c.Id == characterClassId)
            ?? throw new DomainException("Class entry not found on this character.");

        UsedHitDice = Math.Clamp(usedHitDice, 0, classData.ClassLevel);
    }

    // ── Items ────────────────────────────────────────────────────────────────────

    public void AddItem(int itemId, bool equip, bool attune)
    {
        if (_items.Any(i => i.ItemId == itemId))
            throw new DomainException("This item is already in the character's inventory.");

        if (attune && _items.Count(i => i.IsAttuned) >= 3)
            throw new DomainException("A character may attune to at most 3 magic items.");

        _items.Add(new OwnedItem(itemId, equip, attune));
    }

    public void RemoveItem(int itemId)
    {
        var item = _items.FirstOrDefault(i => i.ItemId == itemId)
            ?? throw new DomainException("Item not found in this character's inventory.");
        _items.Remove(item);
    }

    public void ToggleEquipped(int itemId)
    {
        var item = _items.FirstOrDefault(i => i.ItemId == itemId)
            ?? throw new DomainException("Item not found in this character's inventory.");
        item.ToggleEquipped();
    }

    public void ToggleAttuned(int itemId)
    {
        var item = _items.FirstOrDefault(i => i.ItemId == itemId)
            ?? throw new DomainException("Item not found in this character's inventory.");

        if (!item.IsAttuned && _items.Count(i => i.IsAttuned) >= 3)
            throw new DomainException("A character may attune to at most 3 magic items.");

        item.ToggleAttuned();
    }

    public void UpdateItemQuantity(int itemId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.ItemId == itemId)
            ?? throw new DomainException("Item not found in this character's inventory.");
        item.SetQuantity(quantity);
    }

    // ── Spells ───────────────────────────────────────────────────────────────────

    public void AddSpell(int spellId, int spellLevel, bool prepared)
    {
        if (_spells.Any(s => s.SpellId == spellId))
            throw new DomainException("This spell is already in the character's spellbook.");
        _spells.Add(new OwnedSpell(spellId, spellLevel, prepared));
    }

    public void RemoveSpell(int spellId)
    {
        var spell = _spells.FirstOrDefault(s => s.SpellId == spellId)
            ?? throw new DomainException("Spell not found in this character's spellbook.");
        _spells.Remove(spell);
    }

    public void ToggleSpellPrepared(int spellId)
    {
        var spell = _spells.FirstOrDefault(s => s.SpellId == spellId)
            ?? throw new DomainException("Spell not found in this character's spellbook.");
        spell.TogglePrepared();
    }

    public void UpdateSpellSlot(int slotLevel, int remaining)
    {
        var slot = _spellSlots.FirstOrDefault(s => s.SlotLevel == slotLevel)
            ?? throw new DomainException($"No spell slot of level {slotLevel} found.");
        if (remaining < 0 || remaining > slot.TotalSlots)
            throw new DomainException($"Remaining slots must be between 0 and {slot.TotalSlots}.");
        slot.SetRemaining(remaining);
    }

    // ── Feats ────────────────────────────────────────────────────────────────────

    public void AddFeat(int featId)
    {
        if (_feats.Any(f => f.FeatId == featId))
            throw new DomainException("Character already has this feat.");
        _feats.Add(new OwnedFeat(featId));
    }

    public void RemoveFeat(int featId)
    {
        var feat = _feats.FirstOrDefault(f => f.FeatId == featId)
            ?? throw new DomainException("Feat not found on this character.");
        _feats.Remove(feat);
    }

    // ── Features ─────────────────────────────────────────────────────────────────

    public void AddFeature(int persistenceId, string name, string description)
    {
        _features.Add(new CharacterFeature(persistenceId, name, description));
    }

    public void RemoveFeature(int persistenceId)
    {
        var feature = _features.FirstOrDefault(f => f.PersistenceId == persistenceId)
            ?? throw new DomainException("Feature not found on this character.");
        _features.Remove(feature);
    }

    // ── Proficiencies ─────────────────────────────────────────────────────────────

    public void AddSkillProficiency(Skill skill, bool expertise)   => _proficiencies.AddSkill(skill, expertise);
    public void AddWeaponProficiency(string name)                   => _proficiencies.AddWeapon(name);
    public void AddArmorProficiency(string name)                    => _proficiencies.AddArmor(name);
    public void AddLanguage(string name)                            => _proficiencies.AddLanguage(name);
    public void AddInstrument(string name)                          => _proficiencies.AddInstrument(name);
    public void AddSavingThrow(Ability ability)                     => _proficiencies.AddSavingThrow(ability);

    public void RemoveProficiency(ProficiencyType type, string nameOrAbility)
    {
        switch (type)
        {
            case ProficiencyType.Skill:
                if (!Enum.TryParse<Skill>(nameOrAbility, true, out var skill))
                    throw new DomainException($"Unknown skill: {nameOrAbility}");
                _proficiencies.RemoveSkill(skill);
                break;
            case ProficiencyType.Weapon:      _proficiencies.RemoveWeapon(nameOrAbility);      break;
            case ProficiencyType.Armor:       _proficiencies.RemoveArmor(nameOrAbility);       break;
            case ProficiencyType.Language:    _proficiencies.RemoveLanguage(nameOrAbility);    break;
            case ProficiencyType.Instrument:  _proficiencies.RemoveInstrument(nameOrAbility);  break;
            case ProficiencyType.SavingThrow:
                if (!Enum.TryParse<Ability>(nameOrAbility, true, out var ability))
                    throw new DomainException($"Unknown ability: {nameOrAbility}");
                _proficiencies.RemoveSavingThrow(ability);
                break;
            default:
                throw new DomainException($"Unknown proficiency type: {type}");
        }
    }

    // ── Wallet ───────────────────────────────────────────────────────────────────

    public void UpdateWallet(CharacterWallet wallet) => Wallet = wallet;

    // ── Helpers ──────────────────────────────────────────────────────────────────

    private void ApplyAsi(AsiOrFeatChoice choice)
    {
        if (Stats is null) throw new DomainException("Character has no ability scores to improve.");

        if (choice.SingleAbility.HasValue && choice.SingleBonus.HasValue)
        {
            if (choice.SingleBonus.Value != 2)
                throw new DomainException("Single ability improvement must grant +2.");
            Stats = IncrementAbility(Stats, choice.SingleAbility.Value, 2);
        }
        else if (choice.AbilityA.HasValue && choice.AbilityB.HasValue)
        {
            Stats = IncrementAbility(Stats, choice.AbilityA.Value, 1);
            Stats = IncrementAbility(Stats, choice.AbilityB.Value, 1);
        }
        else
        {
            throw new DomainException("Invalid ASI choice: specify a single ability (+2) or two abilities (+1/+1).");
        }
    }

    private static CharacterStats IncrementAbility(CharacterStats stats, Ability ability, int amount) =>
        ability switch
        {
            Ability.Strength     => stats with { Strength     = Math.Min(20, stats.Strength     + amount) },
            Ability.Dexterity    => stats with { Dexterity    = Math.Min(20, stats.Dexterity    + amount) },
            Ability.Constitution => stats with { Constitution = Math.Min(20, stats.Constitution + amount) },
            Ability.Intelligence => stats with { Intelligence = Math.Min(20, stats.Intelligence + amount) },
            Ability.Wisdom       => stats with { Wisdom       = Math.Min(20, stats.Wisdom       + amount) },
            Ability.Charisma     => stats with { Charisma     = Math.Min(20, stats.Charisma     + amount) },
            _                    => throw new DomainException($"Unknown ability: {ability}")
        };
}
