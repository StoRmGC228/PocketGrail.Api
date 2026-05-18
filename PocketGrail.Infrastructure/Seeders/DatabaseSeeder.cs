namespace PocketGrail.Infrastructure.Seeders;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.ClassEntities;
using PocketGrail.Domain.Entities.Enums;
using PocketGrail.Domain.Entities.Proficiencies;

internal sealed class DatabaseSeeder : IDatabaseSeeder
{
    private readonly PocketGrailDbContext _context;

    public DatabaseSeeder(PocketGrailDbContext context) => _context = context;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await SeedClassesAsync(ct);
        await SeedRacesAsync(ct);
        await PatchRaceFlexBonusSlotsAsync(ct);
        await SeedClassSkillChoicesAsync(ct);
    }

    // ── Classes ────────────────────────────────────────────────────────────────

    private async Task SeedClassesAsync(CancellationToken ct)
    {
        if (await _context.Classes.AnyAsync(ct)) return;

        var classes = BuildClasses();
        await _context.Classes.AddRangeAsync(classes, ct);
        await _context.SaveChangesAsync(ct);
    }

    private static List<Class> BuildClasses()
    {
        return
        [
            BuildClass("Barbarian", "d12", null, 2, AsiLevels.Standard,
                ClassSavingThrows.StrCon,
                ClassFeatures.Barbarian,
                ClassProficiencies.Barbarian,
                Subclasses.Barbarian,
                MulticlassPrereqs.Str13,
                SpellSlots.None),

            BuildClass("Bard", "d8", "Charisma", 3, AsiLevels.Standard,
                ClassSavingThrows.DexCha,
                ClassFeatures.Bard,
                ClassProficiencies.Bard,
                Subclasses.Bard,
                MulticlassPrereqs.Cha13,
                SpellSlots.FullCaster),

            BuildClass("Cleric", "d8", "Wisdom", 2, AsiLevels.Standard,
                ClassSavingThrows.WisCha,
                ClassFeatures.Cleric,
                ClassProficiencies.Cleric,
                Subclasses.Cleric,
                MulticlassPrereqs.Wis13,
                SpellSlots.FullCaster),

            BuildClass("Druid", "d8", "Wisdom", 2, AsiLevels.Standard,
                ClassSavingThrows.IntWis,
                ClassFeatures.Druid,
                ClassProficiencies.Druid,
                Subclasses.Druid,
                MulticlassPrereqs.Wis13,
                SpellSlots.FullCaster),

            BuildClass("Fighter", "d10", null, 2, AsiLevels.Fighter,
                ClassSavingThrows.StrCon,
                ClassFeatures.Fighter,
                ClassProficiencies.Fighter,
                Subclasses.Fighter,
                MulticlassPrereqs.Str13OrDex13,
                SpellSlots.None),

            BuildClass("Monk", "d8", null, 2, AsiLevels.Standard,
                ClassSavingThrows.StrDex,
                ClassFeatures.Monk,
                ClassProficiencies.Monk,
                Subclasses.Monk,
                MulticlassPrereqs.Dex13AndWis13,
                SpellSlots.None),

            BuildClass("Paladin", "d10", "Charisma", 2, AsiLevels.Standard,
                ClassSavingThrows.WisCha,
                ClassFeatures.Paladin,
                ClassProficiencies.Paladin,
                Subclasses.Paladin,
                MulticlassPrereqs.Str13AndCha13,
                SpellSlots.HalfCasterPaladin),

            BuildClass("Ranger", "d10", "Wisdom", 3, AsiLevels.Standard,
                ClassSavingThrows.StrDex,
                ClassFeatures.Ranger,
                ClassProficiencies.Ranger,
                Subclasses.Ranger,
                MulticlassPrereqs.Dex13AndWis13,
                SpellSlots.HalfCasterRanger),

            BuildClass("Rogue", "d8", null, 4, AsiLevels.Rogue,
                ClassSavingThrows.DexInt,
                ClassFeatures.Rogue,
                ClassProficiencies.Rogue,
                Subclasses.Rogue,
                MulticlassPrereqs.Dex13,
                SpellSlots.None),

            BuildClass("Sorcerer", "d6", "Charisma", 2, AsiLevels.Standard,
                ClassSavingThrows.ConCha,
                ClassFeatures.Sorcerer,
                ClassProficiencies.Sorcerer,
                Subclasses.Sorcerer,
                MulticlassPrereqs.Cha13,
                SpellSlots.FullCaster),

            BuildClass("Warlock", "d8", "Charisma", 2, AsiLevels.Standard,
                ClassSavingThrows.WisCha,
                ClassFeatures.Warlock,
                ClassProficiencies.Warlock,
                Subclasses.Warlock,
                MulticlassPrereqs.Cha13,
                SpellSlots.Warlock),

            BuildClass("Wizard", "d6", "Intelligence", 2, AsiLevels.Standard,
                ClassSavingThrows.IntWis,
                ClassFeatures.Wizard,
                ClassProficiencies.Wizard,
                Subclasses.Wizard,
                MulticlassPrereqs.Int13,
                SpellSlots.FullCaster),

            BuildClass("Artificer", "d8", "Intelligence", 2, AsiLevels.Artificer,
                ClassSavingThrows.ConInt,
                ClassFeatures.Artificer,
                ClassProficiencies.Artificer,
                Subclasses.Artificer,
                MulticlassPrereqs.Int13,
                SpellSlots.HalfCasterArtificer),
        ];
    }

    private static Class BuildClass(
        string name, string hitDice, string? spellAbility, int skillChoiceCount,
        int[] asiLevels,
        (Ability, Ability)[] savingThrows,
        (string Name, string Desc, int Level)[] features,
        (string Name, string Type)[] proficiencies,
        (string Name, string? Desc)[] subclasses,
        (Ability Ability, int Score)[] prereqs,
        (int ClassLevel, int SpellSlotLevel, int TotalSlots)[] spellSlots)
    {
        var now = DateTime.UtcNow;
        var cls = new Class
        {
            Name = name,
            HitDice = hitDice,
            SpellAbility = spellAbility ?? string.Empty,
            SkillChoiceCount = skillChoiceCount,
            ShortDesсription = string.Empty,
            CreatedAt = now,
            UpdatedAt = now
        };

        // Class features
        var allFeatures = features.ToList();
        foreach (var level in asiLevels)
            allFeatures.Add(("Ability Score Improvement",
                "Increase one ability score by 2, or two ability scores by 1 each (max 20). Alternatively, gain a feat.",
                level));

        cls.ClassFeatures = allFeatures
            .Select(f => new ClassFeature
            {
                Name = f.Name,
                Description = f.Desc,
                GainingLevel = f.Level,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        // Level-1 proficiency grants stored on a special "Starting Proficiencies" ClassFeature
        var weaponProfs = proficiencies.Where(p => p.Type == "weapon")
            .Select(p => new WeaponProficiency { Name = p.Name, CreatedAt = now, UpdatedAt = now })
            .ToList();
        var armorProfs = proficiencies.Where(p => p.Type == "armor")
            .Select(p => new ArmorProficiency { Name = p.Name, CreatedAt = now, UpdatedAt = now })
            .ToList();

        if (weaponProfs.Count > 0 || armorProfs.Count > 0)
        {
            var profFeature = new ClassFeature
            {
                Name = "Starting Proficiencies",
                Description = $"{name} starting weapon and armor proficiencies.",
                GainingLevel = 1,
                WeaponGrants = weaponProfs,
                ArmorGrants = armorProfs,
                CreatedAt = now,
                UpdatedAt = now
            };
            ((List<ClassFeature>)cls.ClassFeatures).Add(profFeature);
        }

        // Saving throws
        cls.SavingThrows = savingThrows
            .Select(st => new ClassSavingThrowProficiency
            {
                Ability = st.Item1,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        // Subclasses
        cls.Subclasses = subclasses
            .Select(s => new Subclass
            {
                Name = s.Name,
                ShortDescription = s.Desc,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        // Multiclass prerequisites
        cls.MulticlassPrerequisites = prereqs
            .Select(p => new MulticlassPrerequisite
            {
                RequiredAbility = p.Ability,
                MinimumScore = p.Score,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        // Spell slot templates
        cls.SpellSlotTemplates = spellSlots
            .Select(s => new ClassSpellSlotTemplate
            {
                ClassLevel = s.ClassLevel,
                SpellSlotLevel = s.SpellSlotLevel,
                TotalSlots = s.TotalSlots,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        return cls;
    }

    // ── Races ──────────────────────────────────────────────────────────────────

    private async Task SeedRacesAsync(CancellationToken ct)
    {
        if (await _context.Races.AnyAsync(ct)) return;

        var races = BuildRaces();
        await _context.Races.AddRangeAsync(races, ct);
        await _context.SaveChangesAsync(ct);
    }

    private static List<Race> BuildRaces()
    {
        var now = DateTime.UtcNow;

        return
        [
            BuildRace("Human", 30, 1,1,1,1,1,1, 0, now,
                languages: ["Common", "One of your choice"],
                weapons: [],
                features:
                [
                    ("Extra Language", "You can speak, read, and write one extra language of your choice."),
                    ("Versatile", "You gain +1 to all ability scores."),
                ]),

            BuildRace("Elf", 30, 0,2,0,0,0,0, 0, now,
                languages: ["Common", "Elvish"],
                weapons: [],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Keen Senses", "You have proficiency in the Perception skill."),
                    ("Fey Ancestry", "Advantage on saving throws against being charmed. Magic can't put you to sleep."),
                    ("Trance", "You don't need to sleep. You meditate deeply for 4 hours a day."),
                ]),

            BuildRace("High Elf", 30, 0,2,0,1,0,0, 0, now,
                languages: ["Common", "Elvish", "One of your choice"],
                weapons: ["Longsword", "Shortsword", "Shortbow", "Longbow"],
                features:
                [
                    ("Darkvision", "You have superior vision in dark and dim conditions. 60 ft."),
                    ("Keen Senses", "You have proficiency in the Perception skill."),
                    ("Fey Ancestry", "Advantage on saving throws against being charmed."),
                    ("Trance", "You meditate for 4 hours instead of sleeping."),
                    ("Cantrip", "You know one cantrip from the wizard spell list. INT is your spellcasting ability for it."),
                    ("Extra Language", "You can speak, read, and write one extra language of your choice."),
                ]),

            BuildRace("Wood Elf", 35, 0,2,0,0,1,0, 0, now,
                languages: ["Common", "Elvish"],
                weapons: ["Longsword", "Shortsword", "Shortbow", "Longbow"],
                features:
                [
                    ("Darkvision", "You have superior vision in dark and dim conditions. 60 ft."),
                    ("Keen Senses", "You have proficiency in the Perception skill."),
                    ("Fey Ancestry", "Advantage on saving throws against being charmed."),
                    ("Trance", "You meditate for 4 hours instead of sleeping."),
                    ("Mask of the Wild", "You can attempt to hide even when only lightly obscured by natural phenomena."),
                    ("Fleet of Foot", "Your base walking speed increases to 35 feet."),
                ]),

            BuildRace("Dark Elf", 30, 0,2,0,0,0,0, 0, now,
                languages: ["Common", "Elvish"],
                weapons: [],
                features:
                [
                    ("Superior Darkvision", "Your darkvision has a radius of 120 feet."),
                    ("Sunlight Sensitivity", "Disadvantage on attack rolls and Perception checks in direct sunlight."),
                    ("Drow Magic", "You know Dancing Lights. At level 3: Faerie Fire; level 5: Darkness. CHA is spellcasting ability."),
                    ("Fey Ancestry", "Advantage on saving throws against being charmed."),
                ]),

            BuildRace("Dwarf", 25, 0,0,2,0,1,0, 0, now,
                languages: ["Common", "Dwarvish"],
                weapons: ["Battleaxe", "Handaxe", "Light hammer", "Warhammer"],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Dwarven Resilience", "Advantage on saving throws against poison. Resistance against poison damage."),
                    ("Dwarven Combat Training", "Proficiency with battleaxe, handaxe, light hammer, and warhammer."),
                    ("Stonecunning", "Double proficiency on History checks about stonework."),
                    ("Dwarven Toughness", "Your HP maximum increases by 1 per level."),
                ]),

            BuildRace("Mountain Dwarf", 25, 2,0,2,0,0,0, 0, now,
                languages: ["Common", "Dwarvish"],
                weapons: ["Battleaxe", "Handaxe", "Light hammer", "Warhammer"],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Dwarven Resilience", "Advantage on saving throws against poison."),
                    ("Dwarven Combat Training", "Proficiency with battleaxe, handaxe, light hammer, warhammer."),
                    ("Stonecunning", "Double proficiency on History checks about stonework."),
                    ("Dwarven Armor Training", "Proficiency with light and medium armor."),
                ]),

            BuildRace("Halfling", 25, 0,2,0,0,0,0, 0, now,
                languages: ["Common", "Halfling"],
                weapons: [],
                features:
                [
                    ("Lucky", "When you roll a 1 on the d20, reroll and use the new roll."),
                    ("Brave", "Advantage on saving throws against being frightened."),
                    ("Halfling Nimbleness", "You can move through the space of any creature larger than you."),
                ]),

            BuildRace("Half-Elf", 30, 0,0,0,0,0,2, 2, now,
                languages: ["Common", "Elvish", "One of your choice"],
                weapons: [],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Fey Ancestry", "Advantage on saving throws against being charmed."),
                    ("Skill Versatility", "Proficiency in two skills of your choice."),
                ],
                flexBonusSlots: [1, 1]),

            BuildRace("Half-Orc", 30, 2,0,1,0,0,0, 0, now,
                languages: ["Common", "Orc"],
                weapons: [],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Menacing", "You gain proficiency in the Intimidation skill."),
                    ("Relentless Endurance", "Drop to 1 HP instead of 0 once per long rest."),
                    ("Savage Attacks", "On a critical hit with melee weapon, roll one damage die one additional time."),
                ]),

            BuildRace("Dragonborn", 30, 2,0,0,0,0,0, 0, now,
                languages: ["Common", "Draconic"],
                weapons: [],
                features:
                [
                    ("Draconic Ancestry", "You have draconic ancestry. Choose a dragon type for your breath weapon and resistance."),
                    ("Breath Weapon", "Exhale destructive energy as an action. Area and damage type determined by ancestry."),
                    ("Damage Resistance", "Resistance to the damage type of your draconic ancestry."),
                ]),

            BuildRace("Gnome", 25, 0,0,0,2,0,0, 0, now,
                languages: ["Common", "Gnomish"],
                weapons: [],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Gnome Cunning", "Advantage on INT, WIS, and CHA saving throws against magic."),
                ]),

            BuildRace("Tiefling", 30, 0,0,0,1,0,2, 0, now,
                languages: ["Common", "Infernal"],
                weapons: [],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Hellish Resistance", "You have resistance to fire damage."),
                    ("Infernal Legacy", "You know Thaumaturgy. At level 3: Hellish Rebuke; level 5: Darkness."),
                ]),

            BuildRace("Aasimar", 30, 0,0,0,0,0,0, 0, now,
                languages: ["Common", "Celestial"],
                weapons: [],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Celestial Resistance", "Resistance to necrotic and radiant damage."),
                    ("Healing Hands", "Touch a creature to heal HP equal to your level. Once per long rest."),
                    ("Light Bearer", "You know the Light cantrip. CHA is your spellcasting ability for it."),
                ]),

            BuildRace("Tabaxi", 30, 0,2,0,0,0,0, 0, now,
                languages: ["Common", "One of your choice"],
                weapons: [],
                features:
                [
                    ("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                    ("Feline Agility", "You can double your speed until end of turn. Usable again after moving 0 feet."),
                    ("Cat's Claws", "Climbing speed equals walking speed. Claws deal 1d4 + STR slashing damage."),
                    ("Cat's Talent", "Proficiency in Perception and Stealth."),
                ]),
        ];
    }

    private static Race BuildRace(
        string name, int speed,
        int str, int dex, int con, int @int, int wis, int cha,
        int flexibleBonusPoints,
        DateTime now,
        string[] languages,
        string[] weapons,
        (string Name, string Desc)[] features,
        int[]? flexBonusSlots = null)
    {
        return new Race
        {
            Name = name,
            BaseSpeed = speed,
            StrBonus = str,
            DexBonus = dex,
            ConBonus = con,
            IntBonus = @int,
            WisBonus = wis,
            ChaBonus = cha,
            FlexibleBonusPoints = flexibleBonusPoints,
            FlexBonusSlots = flexBonusSlots?.ToList() ?? [],
            LanguageGrants = languages
                .Select(l => new Language { Name = l, CreatedAt = now, UpdatedAt = now })
                .ToList(),
            WeaponGrants = weapons
                .Select(w => new WeaponProficiency { Name = w, CreatedAt = now, UpdatedAt = now })
                .ToList(),
            Features = features
                .Select(f => new RaceFeature
                {
                    Name = f.Name,
                    Description = f.Desc,
                    CreatedAt = now,
                    UpdatedAt = now
                })
                .ToList(),
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    // ── Race flex bonus slot patches ──────────────────────────────────────────

    private async Task PatchRaceFlexBonusSlotsAsync(CancellationToken ct)
    {
        // Map of race name → correct FlexBonusSlots (only races with flexible bonuses)
        var patches = new Dictionary<string, List<int>>
        {
            ["Half-Elf"] = [1, 1],
        };

        var names = patches.Keys.ToList();
        var races = await _context.Races
            .Where(r => names.Contains(r.Name) && r.FlexBonusSlots.Count == 0)
            .ToListAsync(ct);

        if (races.Count == 0) return;

        foreach (var race in races)
            race.FlexBonusSlots = patches[race.Name];

        await _context.SaveChangesAsync(ct);
    }

    // ── Class skill choices ────────────────────────────────────────────────────

    private async Task SeedClassSkillChoicesAsync(CancellationToken ct)
    {
        if (await _context.ClassStartSkillProficiencies.AnyAsync(ct)) return;

        var classSkillMap = new Dictionary<string, Skill[]>
        {
            ["Barbarian"] = [Skill.AnimalHandling, Skill.Athletics, Skill.Intimidation, Skill.Nature, Skill.Perception, Skill.Survival],
            ["Bard"]      = [Skill.Acrobatics, Skill.AnimalHandling, Skill.Arcana, Skill.Athletics, Skill.Deception, Skill.History, Skill.Insight, Skill.Intimidation, Skill.Investigation, Skill.Medicine, Skill.Nature, Skill.Perception, Skill.Performance, Skill.Persuasion, Skill.Religion, Skill.SleightOfHand, Skill.Stealth, Skill.Survival],
            ["Cleric"]    = [Skill.History, Skill.Insight, Skill.Medicine, Skill.Persuasion, Skill.Religion],
            ["Druid"]     = [Skill.Arcana, Skill.AnimalHandling, Skill.Insight, Skill.Medicine, Skill.Nature, Skill.Perception, Skill.Religion, Skill.Survival],
            ["Fighter"]   = [Skill.Acrobatics, Skill.AnimalHandling, Skill.Athletics, Skill.History, Skill.Insight, Skill.Intimidation, Skill.Perception, Skill.Survival],
            ["Monk"]      = [Skill.Acrobatics, Skill.Athletics, Skill.History, Skill.Insight, Skill.Religion, Skill.Stealth],
            ["Paladin"]   = [Skill.Athletics, Skill.Insight, Skill.Intimidation, Skill.Medicine, Skill.Persuasion, Skill.Religion],
            ["Ranger"]    = [Skill.AnimalHandling, Skill.Athletics, Skill.Insight, Skill.Investigation, Skill.Nature, Skill.Perception, Skill.Stealth, Skill.Survival],
            ["Rogue"]     = [Skill.Acrobatics, Skill.Athletics, Skill.Deception, Skill.Insight, Skill.Intimidation, Skill.Investigation, Skill.Perception, Skill.Performance, Skill.Persuasion, Skill.SleightOfHand, Skill.Stealth],
            ["Sorcerer"]  = [Skill.Arcana, Skill.Deception, Skill.Insight, Skill.Intimidation, Skill.Persuasion, Skill.Religion],
            ["Warlock"]   = [Skill.Arcana, Skill.Deception, Skill.History, Skill.Intimidation, Skill.Investigation, Skill.Nature, Skill.Religion],
            ["Wizard"]    = [Skill.Arcana, Skill.History, Skill.Insight, Skill.Investigation, Skill.Medicine, Skill.Religion],
            ["Artificer"] = [Skill.Arcana, Skill.History, Skill.Investigation, Skill.Medicine, Skill.Nature, Skill.Perception, Skill.SleightOfHand],
        };

        var classNames = classSkillMap.Keys.ToList();
        var classes = await _context.Classes
            .Where(c => classNames.Contains(c.Name))
            .ToListAsync(ct);

        var now = DateTime.UtcNow;
        foreach (var cls in classes)
        {
            if (!classSkillMap.TryGetValue(cls.Name, out var skills)) continue;
            foreach (var skill in skills)
            {
                _context.ClassStartSkillProficiencies.Add(new ClassStartSkillProficiency
                {
                    ClassId   = cls.Id,
                    Skill     = skill,
                    CreatedAt = now,
                    UpdatedAt = now,
                });
            }
        }

        await _context.SaveChangesAsync(ct);
    }

    // ── Data constants ─────────────────────────────────────────────────────────

    private static class AsiLevels
    {
        public static readonly int[] Standard = [4, 8, 12, 16, 19];
        public static readonly int[] Fighter   = [4, 6, 8, 12, 14, 16, 19];
        public static readonly int[] Rogue     = [4, 8, 10, 12, 16, 18];
        public static readonly int[] Artificer = [4, 8, 12, 16];
    }

    private static class ClassSavingThrows
    {
        public static readonly (Ability, Ability)[] StrCon  = [(Ability.Strength, Ability.Constitution)];
        public static readonly (Ability, Ability)[] DexCha  = [(Ability.Dexterity, Ability.Charisma)];
        public static readonly (Ability, Ability)[] WisCha  = [(Ability.Wisdom, Ability.Charisma)];
        public static readonly (Ability, Ability)[] IntWis  = [(Ability.Intelligence, Ability.Wisdom)];
        public static readonly (Ability, Ability)[] StrDex  = [(Ability.Strength, Ability.Dexterity)];
        public static readonly (Ability, Ability)[] DexInt  = [(Ability.Dexterity, Ability.Intelligence)];
        public static readonly (Ability, Ability)[] ConCha  = [(Ability.Constitution, Ability.Charisma)];
        public static readonly (Ability, Ability)[] ConInt  = [(Ability.Constitution, Ability.Intelligence)];
    }

    private static class MulticlassPrereqs
    {
        public static readonly (Ability, int)[] Str13        = [(Ability.Strength, 13)];
        public static readonly (Ability, int)[] Cha13        = [(Ability.Charisma, 13)];
        public static readonly (Ability, int)[] Wis13        = [(Ability.Wisdom, 13)];
        public static readonly (Ability, int)[] Int13        = [(Ability.Intelligence, 13)];
        public static readonly (Ability, int)[] Dex13        = [(Ability.Dexterity, 13)];
        public static readonly (Ability, int)[] Str13OrDex13 = [(Ability.Strength, 13)]; // OR — store as two separate prereqs; service checks if EITHER meets
        public static readonly (Ability, int)[] Dex13AndWis13  = [(Ability.Dexterity, 13), (Ability.Wisdom, 13)];
        public static readonly (Ability, int)[] Str13AndCha13  = [(Ability.Strength, 13), (Ability.Charisma, 13)];
    }

    private static class ClassProficiencies
    {
        public static (string, string)[] Barbarian => [
            ("Simple weapons", "weapon"), ("Martial weapons", "weapon"),
            ("Light armor", "armor"), ("Medium armor", "armor"), ("Shields", "armor"),
        ];
        public static (string, string)[] Bard => [
            ("Simple weapons", "weapon"), ("Hand crossbows", "weapon"),
            ("Longswords", "weapon"), ("Rapiers", "weapon"), ("Shortswords", "weapon"),
            ("Light armor", "armor"),
        ];
        public static (string, string)[] Cleric => [
            ("Simple weapons", "weapon"),
            ("Light armor", "armor"), ("Medium armor", "armor"), ("Shields", "armor"),
        ];
        public static (string, string)[] Druid => [
            ("Clubs", "weapon"), ("Daggers", "weapon"), ("Darts", "weapon"),
            ("Javelins", "weapon"), ("Maces", "weapon"), ("Quarterstaffs", "weapon"),
            ("Scimitars", "weapon"), ("Sickles", "weapon"), ("Slings", "weapon"), ("Spears", "weapon"),
            ("Light armor", "armor"), ("Medium armor", "armor"), ("Shields", "armor"),
        ];
        public static (string, string)[] Fighter => [
            ("Simple weapons", "weapon"), ("Martial weapons", "weapon"),
            ("All armor", "armor"), ("Shields", "armor"),
        ];
        public static (string, string)[] Monk => [
            ("Simple weapons", "weapon"), ("Shortswords", "weapon"),
        ];
        public static (string, string)[] Paladin => [
            ("Simple weapons", "weapon"), ("Martial weapons", "weapon"),
            ("All armor", "armor"), ("Shields", "armor"),
        ];
        public static (string, string)[] Ranger => [
            ("Simple weapons", "weapon"), ("Martial weapons", "weapon"),
            ("Light armor", "armor"), ("Medium armor", "armor"), ("Shields", "armor"),
        ];
        public static (string, string)[] Rogue => [
            ("Simple weapons", "weapon"), ("Hand crossbows", "weapon"),
            ("Longswords", "weapon"), ("Rapiers", "weapon"), ("Shortswords", "weapon"),
            ("Light armor", "armor"),
        ];
        public static (string, string)[] Sorcerer => [
            ("Daggers", "weapon"), ("Darts", "weapon"), ("Slings", "weapon"),
            ("Quarterstaffs", "weapon"), ("Light crossbows", "weapon"),
        ];
        public static (string, string)[] Warlock => [
            ("Simple weapons", "weapon"), ("Light armor", "armor"),
        ];
        public static (string, string)[] Wizard => [
            ("Daggers", "weapon"), ("Darts", "weapon"), ("Slings", "weapon"),
            ("Quarterstaffs", "weapon"), ("Light crossbows", "weapon"),
        ];
        public static (string, string)[] Artificer => [
            ("Simple weapons", "weapon"), ("Firearms", "weapon"),
            ("Light armor", "armor"), ("Medium armor", "armor"), ("Shields", "armor"),
        ];
    }

    private static class ClassFeatures
    {
        public static (string, string, int)[] Barbarian =>
        [
            ("Rage", "Bonus action to enter a rage. Advantage on STR checks/saves, bonus damage, resistance to bludgeoning/piercing/slashing.", 1),
            ("Unarmored Defense", "While not wearing armor, AC = 10 + DEX mod + CON mod.", 1),
            ("Reckless Attack", "When making your first attack, you can attack recklessly — advantage on melee STR rolls, but attacks against you also have advantage.", 2),
            ("Danger Sense", "Advantage on DEX saving throws against effects you can see, when not blinded/deafened/incapacitated.", 2),
            ("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
            ("Fast Movement", "Your speed increases by 10 ft. while not wearing heavy armor.", 5),
            ("Feral Instinct", "Advantage on initiative rolls. If surprised, you can still move and attack on the first turn.", 7),
            ("Brutal Critical", "Roll one additional weapon damage die on a critical hit.", 9),
            ("Relentless Rage", "When you drop to 0 HP while raging, make DC 10 CON save to stay at 1 HP instead.", 11),
        ];

        public static (string, string, int)[] Bard =>
        [
            ("Bardic Inspiration", "Bonus action to give a creature within 60 ft. a Bardic Inspiration die (d6).", 1),
            ("Spellcasting", "You can cast bard spells using CHA as your spellcasting ability.", 1),
            ("Jack of All Trades", "Add half your proficiency bonus to ability checks that don't already include it.", 2),
            ("Song of Rest", "During a short rest, allies regain 1d6 extra HP.", 2),
            ("Expertise", "Choose two skill proficiencies. Your proficiency bonus is doubled for these.", 3),
            ("Font of Inspiration", "You regain Bardic Inspiration uses on a short or long rest.", 5),
            ("Countercharm", "Start a performance giving friendly creatures within 30 ft. advantage vs frightened/charmed.", 6),
            ("Magical Secrets", "Choose two spells from any class spell list as bard spells.", 10),
        ];

        public static (string, string, int)[] Cleric =>
        [
            ("Spellcasting", "You can cast cleric spells using WIS as your spellcasting ability.", 1),
            ("Divine Domain", "Choose a divine domain. You gain domain spells and domain features.", 1),
            ("Channel Divinity", "Channel divine energy to fuel magical effects. Once per rest.", 2),
            ("Turn Undead", "As an action, present your holy symbol and speak a prayer censuring the undead.", 2),
            ("Destroy Undead", "When an undead fails Turn Undead, it is instantly destroyed if CR is low enough.", 5),
            ("Divine Intervention", "Call on your deity to intervene. Succeeds on a roll equal to your cleric level on a d100.", 10),
        ];

        public static (string, string, int)[] Druid =>
        [
            ("Druidic", "You know Druidic, the secret language of druids.", 1),
            ("Spellcasting", "You can cast druid spells using WIS as your spellcasting ability.", 1),
            ("Wild Shape", "Action to magically assume the shape of a beast you have seen before.", 2),
            ("Timeless Body", "For every 10 years, your body ages only 1 year.", 18),
            ("Beast Spells", "You can cast many druid spells in Wild Shape form.", 18),
        ];

        public static (string, string, int)[] Fighter =>
        [
            ("Fighting Style", "Choose a fighting style specialty (Archery, Defense, Dueling, etc.).", 1),
            ("Second Wind", "Bonus action to regain 1d10 + fighter level HP once per short/long rest.", 1),
            ("Action Surge", "On your turn, take one additional action once per short/long rest.", 2),
            ("Martial Archetype", "Choose a martial archetype that shapes the kind of fighter you want to be.", 3),
            ("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
            ("Indomitable", "Reroll a saving throw that you fail once per long rest.", 9),
        ];

        public static (string, string, int)[] Monk =>
        [
            ("Unarmored Defense", "While wearing no armor and not wielding a shield, AC = 10 + DEX mod + WIS mod.", 1),
            ("Martial Arts", "Use DEX instead of STR for unarmed strikes and monk weapons.", 1),
            ("Ki", "Gain ki points equal to your monk level to fuel special monk features.", 2),
            ("Unarmored Movement", "Speed increases by 10 ft. when not wearing armor or a shield.", 2),
            ("Deflect Missiles", "Reaction to deflect or catch a missile when hit by a ranged weapon attack.", 3),
            ("Slow Fall", "Reaction to reduce falling damage by five times your monk level.", 4),
            ("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
            ("Stunning Strike", "Spend 1 ki after hitting with a melee weapon attack to force CON save or be stunned.", 5),
        ];

        public static (string, string, int)[] Paladin =>
        [
            ("Divine Sense", "Action to detect celestials, fiends, and undead within 60 ft.", 1),
            ("Lay on Hands", "Pool of healing HP = paladin level × 5.", 1),
            ("Fighting Style", "Choose a fighting style specialty.", 2),
            ("Spellcasting", "You can cast paladin spells using CHA as your spellcasting ability.", 2),
            ("Divine Smite", "When you hit with a melee weapon attack, expend a spell slot to deal radiant damage.", 2),
            ("Sacred Oath", "Swear a sacred oath granting oath spells and Channel Divinity.", 3),
            ("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
            ("Aura of Protection", "When you or a friendly creature within 10 ft. makes a saving throw, add CHA mod.", 6),
        ];

        public static (string, string, int)[] Ranger =>
        [
            ("Favored Enemy", "Choose a type of favored enemy. Advantage on Survival checks to track them.", 1),
            ("Natural Explorer", "Choose a favored terrain type with travel and exploration benefits.", 1),
            ("Fighting Style", "Choose a fighting style specialty.", 2),
            ("Spellcasting", "You can cast ranger spells using WIS as your spellcasting ability.", 2),
            ("Primeval Awareness", "Expend a spell slot to magically sense certain creature types nearby.", 3),
            ("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
        ];

        public static (string, string, int)[] Rogue =>
        [
            ("Expertise", "Choose two skill proficiencies. Your proficiency bonus is doubled for these.", 1),
            ("Sneak Attack", "Once per turn, deal extra 1d6 damage when you have advantage or an ally is adjacent to the target.", 1),
            ("Thieves' Cant", "Secret language allowing you to hide messages in normal conversation.", 1),
            ("Cunning Action", "Bonus action to Dash, Disengage, or Hide.", 2),
            ("Uncanny Dodge", "Reaction to halve damage from one attacker you can see.", 5),
            ("Evasion", "When you make a DEX save for half damage, take no damage on success.", 7),
            ("Reliable Talent", "Whenever you add proficiency to an ability check, treat a roll of 9 or lower as 10.", 11),
        ];

        public static (string, string, int)[] Sorcerer =>
        [
            ("Spellcasting", "You can cast sorcerer spells using CHA as your spellcasting ability.", 1),
            ("Sorcerous Origin", "Choose a sorcerous origin describing the source of your innate magical power.", 1),
            ("Font of Magic", "Gain sorcery points equal to your sorcerer level to fuel metamagic.", 2),
            ("Metamagic", "Choose two metamagic options to shape your spells.", 3),
            ("Sorcerous Restoration", "Regain 4 expended sorcery points on a short rest.", 20),
        ];

        public static (string, string, int)[] Warlock =>
        [
            ("Otherworldly Patron", "Choose a patron whose power you have bargained with.", 1),
            ("Pact Magic", "Use CHA to cast warlock spells. Regain spell slots on short rest.", 1),
            ("Eldritch Invocations", "Gain fragments of forbidden knowledge granting permanent magical abilities.", 2),
            ("Pact Boon", "Your otherworldly patron gives you a gift for your loyal service.", 3),
            ("Mystic Arcanum", "Choose a 6th-level spell from the warlock list as a magical secret.", 11),
        ];

        public static (string, string, int)[] Wizard =>
        [
            ("Spellcasting", "You can cast wizard spells using INT as your spellcasting ability.", 1),
            ("Arcane Recovery", "Once per day on a short rest, recover spell slots with total levels ≤ half your wizard level.", 1),
            ("Arcane Tradition", "Choose an arcane tradition that shapes how you practice magic.", 2),
            ("Spell Mastery", "Choose a 1st-level and a 2nd-level spell. Cast them at lowest level without expending a slot.", 18),
            ("Signature Spells", "Choose two 3rd-level wizard spells as signature spells you can cast without expending slots.", 20),
        ];

        public static (string, string, int)[] Artificer =>
        [
            ("Magical Tinkering", "Imbue a tiny object with one of several magical properties.", 1),
            ("Spellcasting", "You can cast artificer spells using INT as your spellcasting ability.", 1),
            ("Infuse Item", "Infuse mundane items with magical infusions.", 2),
            ("The Right Tool for the Job", "Create one set of artisan's tools you're missing using a short rest.", 3),
            ("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
            ("Tool Expertise", "Your proficiency bonus is doubled for ability checks using tools you are proficient with.", 6),
        ];
    }

    private static class Subclasses
    {
        public static (string, string?)[] Barbarian => [("Berserker", "Channels rage into frenzied attacks."), ("Totem Warrior", "Draws power from animal spirits.")];
        public static (string, string?)[] Bard => [("College of Lore", "Focuses on knowledge and secrets."), ("College of Valor", "Inspires allies in combat.")];
        public static (string, string?)[] Cleric => [("Life Domain", "Healing and vitality."), ("War Domain", "Martial prowess."), ("Knowledge Domain", "Wisdom and secrets."), ("Nature Domain", "Power over nature.")];
        public static (string, string?)[] Druid => [("Circle of the Land", "Draws power from natural terrain."), ("Circle of the Moon", "Transforms into powerful beasts.")];
        public static (string, string?)[] Fighter => [("Champion", "Exceptional athletic talent."), ("Battle Master", "Combat maneuvers and tactics."), ("Eldritch Knight", "Combines martial prowess with magic.")];
        public static (string, string?)[] Monk => [("Way of the Open Hand", "Master of unarmed combat."), ("Way of Shadow", "Uses shadows to move unseen."), ("Way of the Four Elements", "Channels elemental forces.")];
        public static (string, string?)[] Paladin => [("Oath of Devotion", "Sacred oath to the tenets of good."), ("Oath of the Ancients", "Protects the light of nature."), ("Oath of Vengeance", "Punishes evildoers.")];
        public static (string, string?)[] Ranger => [("Hunter", "Specializes in hunting specific prey."), ("Beast Master", "Forms a bond with a beast companion.")];
        public static (string, string?)[] Rogue => [("Thief", "Master of theft and infiltration."), ("Arcane Trickster", "Uses illusion and enchantment magic."), ("Assassin", "Specializes in disguise and killing.")];
        public static (string, string?)[] Sorcerer => [("Wild Magic", "Unpredictable magical surges."), ("Draconic Bloodline", "Power inherited from a dragon ancestor.")];
        public static (string, string?)[] Warlock => [("The Archfey", "Bargained with a powerful fey being."), ("The Fiend", "Bargained with a powerful fiend."), ("The Great Old One", "Bargained with an unknowable entity.")];
        public static (string, string?)[] Wizard => [("School of Abjuration", null), ("School of Conjuration", null), ("School of Divination", null), ("School of Evocation", null), ("School of Illusion", null), ("School of Necromancy", null), ("School of Transmutation", null)];
        public static (string, string?)[] Artificer => [("Alchemist", "Creates magical potions and elixirs."), ("Artillerist", "Specializes in magical artillery."), ("Battle Smith", "Uses a steel defender construct in combat.")];
    }

    // ── Spell slot tables ──────────────────────────────────────────────────────

    private static class SpellSlots
    {
        // Full caster: Bard, Cleric, Druid, Sorcerer, Wizard
        // Format: (classLevel, spellSlotLevel, totalSlots)
        public static (int, int, int)[] None => [];

        public static (int, int, int)[] FullCaster =>
        [
            (1,1,2),
            (2,1,3),
            (3,1,4),(3,2,2),
            (4,1,4),(4,2,3),
            (5,1,4),(5,2,3),(5,3,2),
            (6,1,4),(6,2,3),(6,3,3),
            (7,1,4),(7,2,3),(7,3,3),(7,4,1),
            (8,1,4),(8,2,3),(8,3,3),(8,4,2),
            (9,1,4),(9,2,3),(9,3,3),(9,4,3),(9,5,1),
            (10,1,4),(10,2,3),(10,3,3),(10,4,3),(10,5,2),
            (11,1,4),(11,2,3),(11,3,3),(11,4,3),(11,5,2),(11,6,1),
            (12,1,4),(12,2,3),(12,3,3),(12,4,3),(12,5,2),(12,6,1),
            (13,1,4),(13,2,3),(13,3,3),(13,4,3),(13,5,2),(13,6,1),(13,7,1),
            (14,1,4),(14,2,3),(14,3,3),(14,4,3),(14,5,2),(14,6,1),(14,7,1),
            (15,1,4),(15,2,3),(15,3,3),(15,4,3),(15,5,2),(15,6,1),(15,7,1),(15,8,1),
            (16,1,4),(16,2,3),(16,3,3),(16,4,3),(16,5,2),(16,6,1),(16,7,1),(16,8,1),
            (17,1,4),(17,2,3),(17,3,3),(17,4,3),(17,5,2),(17,6,1),(17,7,1),(17,8,1),(17,9,1),
            (18,1,4),(18,2,3),(18,3,3),(18,4,3),(18,5,3),(18,6,1),(18,7,1),(18,8,1),(18,9,1),
            (19,1,4),(19,2,3),(19,3,3),(19,4,3),(19,5,3),(19,6,2),(19,7,1),(19,8,1),(19,9,1),
            (20,1,4),(20,2,3),(20,3,3),(20,4,3),(20,5,3),(20,6,2),(20,7,2),(20,8,1),(20,9,1),
        ];

        // Half caster (Paladin, Ranger) — slots start at level 2
        public static (int, int, int)[] HalfCasterPaladin =>
        [
            (2,1,2),(3,1,3),(4,1,3),(5,1,4),(5,2,2),(6,1,4),(6,2,2),(7,1,4),(7,2,3),
            (8,1,4),(8,2,3),(9,1,4),(9,2,3),(9,3,2),(10,1,4),(10,2,3),(10,3,2),
            (11,1,4),(11,2,3),(11,3,3),(12,1,4),(12,2,3),(12,3,3),(13,1,4),(13,2,3),(13,3,3),(13,4,1),
            (14,1,4),(14,2,3),(14,3,3),(14,4,1),(15,1,4),(15,2,3),(15,3,3),(15,4,2),
            (16,1,4),(16,2,3),(16,3,3),(16,4,2),(17,1,4),(17,2,3),(17,3,3),(17,4,3),(17,5,1),
            (18,1,4),(18,2,3),(18,3,3),(18,4,3),(18,5,1),(19,1,4),(19,2,3),(19,3,3),(19,4,3),(19,5,2),
            (20,1,4),(20,2,3),(20,3,3),(20,4,3),(20,5,2),
        ];

        public static (int, int, int)[] HalfCasterRanger => HalfCasterPaladin;

        // Artificer — same as half caster but starts at level 2
        public static (int, int, int)[] HalfCasterArtificer => HalfCasterPaladin;

        // Warlock — pact magic (different slot progression)
        public static (int, int, int)[] Warlock =>
        [
            (1,1,1),(2,1,2),(3,2,2),(4,2,2),(5,3,2),(6,3,2),(7,4,2),(8,4,2),(9,5,2),(10,5,2),
            (11,5,3),(12,5,3),(13,5,3),(14,5,3),(15,5,3),(16,5,3),(17,5,4),(18,5,4),(19,5,4),(20,5,4),
        ];
    }
}
