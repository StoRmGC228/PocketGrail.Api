namespace PocketGrail.Application.Data;

public static class DnD5eData
{
    public record FeatureEntry(string Name, string Description, int Level = 1);
    public record ProficiencyEntry(string Name, string Type);

    public static readonly Dictionary<string, string> ClassSpellAbility = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Bard"] = "cha",
        ["Cleric"] = "wis",
        ["Druid"] = "wis",
        ["Paladin"] = "cha",
        ["Ranger"] = "wis",
        ["Sorcerer"] = "cha",
        ["Warlock"] = "cha",
        ["Wizard"] = "int",
        ["Artificer"] = "int",
    };

    public static readonly Dictionary<string, List<FeatureEntry>> ClassFeaturesByLevel =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Barbarian"] =
            [
                new("Rage", "Bonus action to enter a rage. Advantage on STR checks/saves, bonus damage, resistance to bludgeoning/piercing/slashing.", 1),
                new("Unarmored Defense", "While not wearing armor, AC = 10 + DEX mod + CON mod.", 1),
                new("Reckless Attack", "When making your first attack on your turn, you can decide to attack recklessly, gaining advantage on melee attack rolls using STR for that turn, but attack rolls against you have advantage until your next turn.", 2),
                new("Danger Sense", "Advantage on DEX saving throws against effects you can see, when not blinded/deafened/incapacitated.", 2),
                new("Extra Attack", "You can attack twice, instead of once, whenever you take the Attack action on your turn.", 5),
                new("Fast Movement", "Your speed increases by 10 ft. while you aren't wearing heavy armor.", 5),
                new("Feral Instinct", "Advantage on initiative rolls. If surprised, you can move and attack on the first turn.", 7),
                new("Brutal Critical", "You can roll one additional weapon damage die when determining the extra damage for a critical hit.", 9),
                new("Relentless Rage", "When you drop to 0 HP while raging, you can make a DC 10 CON save to stay at 1 HP instead.", 11),
            ],
            ["Bard"] =
            [
                new("Bardic Inspiration", "Bonus action to give a creature within 60 ft. a Bardic Inspiration die (d6). They can add it to one ability check, attack roll, or saving throw within 10 minutes.", 1),
                new("Spellcasting", "You can cast bard spells using CHA as your spellcasting ability.", 1),
                new("Jack of All Trades", "Add half your proficiency bonus (rounded down) to any ability check that doesn't already include your proficiency bonus.", 2),
                new("Song of Rest", "During a short rest, you can use soothing music or oration to help revitalize wounded allies. They regain 1d6 extra HP.", 2),
                new("Expertise", "Choose two skill proficiencies or one skill and Thieves' Tools. Your proficiency bonus is doubled for these.", 3),
                new("Font of Inspiration", "You regain your Bardic Inspiration uses when you finish a short or long rest.", 5),
                new("Countercharm", "Action to start a performance lasting until end of your next turn. Friendly creatures within 30 ft. have advantage on saves vs. being frightened or charmed.", 6),
                new("Magical Secrets", "Choose two spells from any class spell list. They count as bard spells for you.", 10),
            ],
            ["Cleric"] =
            [
                new("Spellcasting", "You can cast cleric spells using WIS as your spellcasting ability.", 1),
                new("Divine Domain", "Choose a divine domain. You gain domain spells and domain features.", 1),
                new("Channel Divinity", "You can channel divine energy to fuel magical effects. Once per rest.", 2),
                new("Turn Undead", "As an action, present your holy symbol and speak a prayer censuring the undead.", 2),
                new("Destroy Undead", "When an undead fails its saving throw against your Turn Undead, the creature is instantly destroyed if its challenge rating is low enough.", 5),
                new("Divine Intervention", "Call on your deity to intervene on your behalf. Succeeds on a roll of your cleric level or lower on a d100.", 10),
            ],
            ["Druid"] =
            [
                new("Druidic", "You know Druidic, the secret language of druids. Can speak to and understand plants and animals.", 1),
                new("Spellcasting", "You can cast druid spells using WIS as your spellcasting ability.", 1),
                new("Wild Shape", "Action to magically assume the shape of a beast you have seen before.", 2),
                new("Timeless Body", "You age more slowly; for every 10 years that pass, your body ages only 1 year.", 18),
                new("Beast Spells", "You can cast many of your druid spells in any shape you assume using Wild Shape.", 18),
            ],
            ["Fighter"] =
            [
                new("Fighting Style", "Choose a fighting style specialty (Archery, Defense, Dueling, etc.).", 1),
                new("Second Wind", "Bonus action to regain 1d10 + fighter level HP once per short/long rest.", 1),
                new("Action Surge", "On your turn, take one additional action once per short/long rest.", 2),
                new("Martial Archetype", "Choose a martial archetype that shapes the kind of fighter you want to be.", 3),
                new("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
                new("Indomitable", "Reroll a saving throw that you fail once per long rest.", 9),
            ],
            ["Monk"] =
            [
                new("Unarmored Defense", "While wearing no armor and not wielding a shield, AC = 10 + DEX mod + WIS mod.", 1),
                new("Martial Arts", "Use DEX instead of STR for unarmed strikes and monk weapons. Can make unarmed strike as bonus action.", 1),
                new("Ki", "Gain ki points equal to your monk level. Spend to fuel special monk features.", 2),
                new("Unarmored Movement", "Speed increases by 10 ft. when not wearing armor or a shield.", 2),
                new("Deflect Missiles", "Reaction to deflect or catch a missile when hit by a ranged weapon attack.", 3),
                new("Slow Fall", "Reaction to reduce falling damage by five times your monk level.", 4),
                new("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
                new("Stunning Strike", "Spend 1 ki after hitting with a melee weapon attack to force CON save or be stunned.", 5),
            ],
            ["Paladin"] =
            [
                new("Divine Sense", "Action to detect celestials, fiends, and undead within 60 ft. Uses per day = 1 + CHA mod.", 1),
                new("Lay on Hands", "Pool of healing HP = paladin level × 5. Touch to restore HP or cure disease/poison.", 1),
                new("Fighting Style", "Choose a fighting style specialty.", 2),
                new("Spellcasting", "You can cast paladin spells using CHA as your spellcasting ability.", 2),
                new("Divine Smite", "When you hit with a melee weapon attack, expend a spell slot to deal radiant damage.", 2),
                new("Sacred Oath", "Swear a sacred oath that binds you as a paladin forever. Grants oath spells and Channel Divinity.", 3),
                new("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
                new("Aura of Protection", "When you or a friendly creature within 10 ft. makes a saving throw, add CHA mod (min +1) to the roll.", 6),
            ],
            ["Ranger"] =
            [
                new("Favored Enemy", "Choose a type of favored enemy. Advantage on Survival checks to track them.", 1),
                new("Natural Explorer", "Choose a favored terrain type. You gain various travel and exploration benefits there.", 1),
                new("Fighting Style", "Choose a fighting style specialty.", 2),
                new("Spellcasting", "You can cast ranger spells using WIS as your spellcasting ability.", 2),
                new("Primeval Awareness", "Expend a spell slot to magically sense the presence of certain creature types nearby.", 3),
                new("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
            ],
            ["Rogue"] =
            [
                new("Expertise", "Choose two skill proficiencies or one skill and Thieves' Tools. Your proficiency bonus is doubled for these.", 1),
                new("Sneak Attack", "Once per turn, deal an extra 1d6 damage when you have advantage or an ally is adjacent to the target. Increases by 1d6 at levels 3, 5, 7, 9, 11, 13, 15, 17, 19.", 1),
                new("Thieves' Cant", "Secret mix of dialect, jargon, and code that allows you to hide messages in seemingly normal conversation.", 1),
                new("Cunning Action", "Bonus action to Dash, Disengage, or Hide.", 2),
                new("Uncanny Dodge", "Reaction to halve damage from one attacker you can see when hit.", 5),
                new("Evasion", "When you make a DEX save to take half damage, take no damage on success and half on failure.", 7),
                new("Reliable Talent", "Whenever you make an ability check that lets you add your proficiency bonus, treat a roll of 9 or lower as 10.", 11),
            ],
            ["Sorcerer"] =
            [
                new("Spellcasting", "You can cast sorcerer spells using CHA as your spellcasting ability.", 1),
                new("Sorcerous Origin", "Choose a sorcerous origin that describes the source of your innate magical power.", 1),
                new("Font of Magic", "Gain sorcery points equal to your sorcerer level. Use to fuel metamagic.", 2),
                new("Metamagic", "Choose two metamagic options to shape your spells.", 3),
                new("Sorcerous Restoration", "Regain 4 expended sorcery points on a short rest.", 20),
            ],
            ["Warlock"] =
            [
                new("Otherworldly Patron", "Choose a patron whose power you have bargained with.", 1),
                new("Pact Magic", "Use CHA to cast warlock spells. Regain spell slots on short rest.", 1),
                new("Eldritch Invocations", "Gain fragments of forbidden knowledge granting permanent magical abilities.", 2),
                new("Pact Boon", "Your otherworldly patron gives you a gift for your loyal service.", 3),
                new("Mystic Arcanum", "Choose a 6th-level spell from the warlock list as a magical secret.", 11),
            ],
            ["Wizard"] =
            [
                new("Spellcasting", "You can cast wizard spells using INT as your spellcasting ability.", 1),
                new("Arcane Recovery", "Once per day on a short rest, recover spell slots with total levels ≤ half your wizard level.", 1),
                new("Arcane Tradition", "Choose an arcane tradition that shapes how you practice magic.", 2),
                new("Spell Mastery", "Choose a 1st-level and a 2nd-level spell. Cast them at lowest level without expending a slot.", 18),
                new("Signature Spells", "Choose two 3rd-level wizard spells as signature spells you can cast without expending slots.", 20),
            ],
            ["Artificer"] =
            [
                new("Magical Tinkering", "Imbue a tiny object with one of several magical properties.", 1),
                new("Spellcasting", "You can cast artificer spells using INT as your spellcasting ability.", 1),
                new("Infuse Item", "Infuse mundane items with magical infusions.", 2),
                new("The Right Tool for the Job", "Create one set of artisan's tools you're missing using a short rest.", 3),
                new("Extra Attack", "You can attack twice whenever you take the Attack action on your turn.", 5),
                new("Tool Expertise", "Your proficiency bonus is doubled for any ability check using a tool you are proficient with.", 6),
            ],
        };

    public static readonly Dictionary<string, List<FeatureEntry>> RaceFeatures =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Human"] =
            [
                new("Extra Language", "You can speak, read, and write one extra language of your choice."),
                new("Versatile", "You gain +1 to all ability scores."),
            ],
            ["Elf"] =
            [
                new("Darkvision", "Accustomed to twilit forests and the night sky, you have superior vision in dark and dim conditions. 60 ft."),
                new("Keen Senses", "You have proficiency in the Perception skill."),
                new("Fey Ancestry", "Advantage on saving throws against being charmed. Magic can't put you to sleep."),
                new("Trance", "You don't need to sleep. You meditate deeply for 4 hours a day (long rest = 4 hours)."),
            ],
            ["High Elf"] =
            [
                new("Darkvision", "You have superior vision in dark and dim conditions. 60 ft."),
                new("Keen Senses", "You have proficiency in the Perception skill."),
                new("Fey Ancestry", "Advantage on saving throws against being charmed. Magic can't put you to sleep."),
                new("Trance", "You meditate for 4 hours instead of sleeping."),
                new("Cantrip", "You know one cantrip of your choice from the wizard spell list. INT is your spellcasting ability for it."),
                new("Extra Language", "You can speak, read, and write one extra language of your choice."),
            ],
            ["Wood Elf"] =
            [
                new("Darkvision", "You have superior vision in dark and dim conditions. 60 ft."),
                new("Keen Senses", "You have proficiency in the Perception skill."),
                new("Fey Ancestry", "Advantage on saving throws against being charmed. Magic can't put you to sleep."),
                new("Trance", "You meditate for 4 hours instead of sleeping."),
                new("Mask of the Wild", "You can attempt to hide even when you are only lightly obscured by foliage, heavy rain, falling snow, mist, and other natural phenomena."),
                new("Fleet of Foot", "Your base walking speed increases to 35 feet."),
            ],
            ["Dark Elf"] =
            [
                new("Superior Darkvision", "Your darkvision has a radius of 120 feet."),
                new("Sunlight Sensitivity", "Disadvantage on attack rolls and Perception checks that rely on sight when you or your target is in direct sunlight."),
                new("Drow Magic", "You know the Dancing Lights cantrip. At level 3 you gain Faerie Fire; at level 5 you gain Darkness. CHA is the spellcasting ability."),
                new("Fey Ancestry", "Advantage on saving throws against being charmed. Magic can't put you to sleep."),
            ],
            ["Dwarf"] =
            [
                new("Darkvision", "Accustomed to life underground. You have superior vision in dark and dim conditions. 60 ft."),
                new("Dwarven Resilience", "Advantage on saving throws against poison. Resistance against poison damage."),
                new("Dwarven Combat Training", "Proficiency with battleaxe, handaxe, light hammer, and warhammer."),
                new("Stonecunning", "Whenever you make a History check related to the origin of stonework, add double your proficiency bonus."),
            ],
            ["Hill Dwarf"] =
            [
                new("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                new("Dwarven Resilience", "Advantage on saving throws against poison. Resistance against poison damage."),
                new("Dwarven Combat Training", "Proficiency with battleaxe, handaxe, light hammer, and warhammer."),
                new("Stonecunning", "Double proficiency on History checks about stonework."),
                new("Dwarven Toughness", "Your HP maximum increases by 1, and it increases by 1 every time you gain a level."),
            ],
            ["Halfling"] =
            [
                new("Lucky", "When you roll a 1 on the d20 for an attack roll, ability check, or saving throw, you can reroll the die and must use the new roll."),
                new("Brave", "Advantage on saving throws against being frightened."),
                new("Halfling Nimbleness", "You can move through the space of any creature that is of a size larger than yours."),
            ],
            ["Half-Elf"] =
            [
                new("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                new("Fey Ancestry", "Advantage on saving throws against being charmed. Magic can't put you to sleep."),
                new("Skill Versatility", "Proficiency in two skills of your choice."),
            ],
            ["Half-Orc"] =
            [
                new("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                new("Menacing", "You gain proficiency in the Intimidation skill."),
                new("Relentless Endurance", "When you are reduced to 0 HP but not killed outright, drop to 1 HP instead. Once per long rest."),
                new("Savage Attacks", "When you score a critical hit with a melee weapon attack, you can roll one of the weapon's damage dice one additional time."),
            ],
            ["Dragonborn"] =
            [
                new("Draconic Ancestry", "You have draconic ancestry. Choose a type of dragon — your breath weapon and damage resistance are determined by the dragon type."),
                new("Breath Weapon", "Use your action to exhale destructive energy. The area, damage type, and saving throw are determined by your draconic ancestry."),
                new("Damage Resistance", "You have resistance to the damage type associated with your draconic ancestry."),
            ],
            ["Gnome"] =
            [
                new("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                new("Gnome Cunning", "Advantage on INT, WIS, and CHA saving throws against magic."),
            ],
            ["Tiefling"] =
            [
                new("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                new("Hellish Resistance", "You have resistance to fire damage."),
                new("Infernal Legacy", "You know the Thaumaturgy cantrip. At level 3 you gain Hellish Rebuke; at level 5 you gain Darkness. CHA is the spellcasting ability."),
            ],
            ["Aasimar"] =
            [
                new("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                new("Celestial Resistance", "Resistance to necrotic and radiant damage."),
                new("Healing Hands", "Action to touch a creature and heal HP equal to your level. Once per long rest."),
                new("Light Bearer", "You know the Light cantrip. CHA is your spellcasting ability for it."),
            ],
            ["Tabaxi"] =
            [
                new("Darkvision", "Superior vision in dark and dim conditions. 60 ft."),
                new("Feline Agility", "Your reflexes and agility allow you to move with a burst of speed. When you move on your turn, you can double your speed until the end of the turn. You can't use this trait again until you move 0 feet on one of your turns."),
                new("Cat's Claws", "Your climbing speed equals your walking speed. Your claws are natural weapons that deal 1d4 + STR mod slashing damage."),
                new("Cat's Talent", "Proficiency in Perception and Stealth."),
            ],
        };

    public static readonly Dictionary<string, List<ProficiencyEntry>> ClassDefaultProficiencies =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Barbarian"] = [
                new("Simple weapons", "weapon"), new("Martial weapons", "weapon"),
                new("Light armor", "armor"), new("Medium armor", "armor"), new("Shields", "armor"),
            ],
            ["Bard"] = [
                new("Simple weapons", "weapon"), new("Hand crossbows", "weapon"),
                new("Longswords", "weapon"), new("Rapiers", "weapon"), new("Shortswords", "weapon"),
                new("Light armor", "armor"),
            ],
            ["Cleric"] = [
                new("Simple weapons", "weapon"),
                new("Light armor", "armor"), new("Medium armor", "armor"), new("Shields", "armor"),
            ],
            ["Druid"] = [
                new("Clubs", "weapon"), new("Daggers", "weapon"), new("Darts", "weapon"),
                new("Javelins", "weapon"), new("Maces", "weapon"), new("Quarterstaffs", "weapon"),
                new("Scimitars", "weapon"), new("Sickles", "weapon"), new("Slings", "weapon"),
                new("Spears", "weapon"),
                new("Light armor", "armor"), new("Medium armor", "armor"), new("Shields", "armor"),
                new("Herbalism kit", "tool"),
            ],
            ["Fighter"] = [
                new("Simple weapons", "weapon"), new("Martial weapons", "weapon"),
                new("All armor", "armor"), new("Shields", "armor"),
            ],
            ["Monk"] = [
                new("Simple weapons", "weapon"), new("Shortswords", "weapon"),
                new("Artisan's tools or musical instrument", "tool"),
            ],
            ["Paladin"] = [
                new("Simple weapons", "weapon"), new("Martial weapons", "weapon"),
                new("All armor", "armor"), new("Shields", "armor"),
            ],
            ["Ranger"] = [
                new("Simple weapons", "weapon"), new("Martial weapons", "weapon"),
                new("Light armor", "armor"), new("Medium armor", "armor"), new("Shields", "armor"),
            ],
            ["Rogue"] = [
                new("Simple weapons", "weapon"), new("Hand crossbows", "weapon"),
                new("Longswords", "weapon"), new("Rapiers", "weapon"), new("Shortswords", "weapon"),
                new("Light armor", "armor"),
                new("Thieves' tools", "tool"),
            ],
            ["Sorcerer"] = [
                new("Daggers", "weapon"), new("Darts", "weapon"), new("Slings", "weapon"),
                new("Quarterstaffs", "weapon"), new("Light crossbows", "weapon"),
            ],
            ["Warlock"] = [
                new("Simple weapons", "weapon"),
                new("Light armor", "armor"),
            ],
            ["Wizard"] = [
                new("Daggers", "weapon"), new("Darts", "weapon"), new("Slings", "weapon"),
                new("Quarterstaffs", "weapon"), new("Light crossbows", "weapon"),
            ],
            ["Artificer"] = [
                new("Simple weapons", "weapon"), new("Firearms", "weapon"),
                new("Light armor", "armor"), new("Medium armor", "armor"), new("Shields", "armor"),
                new("Thieves' tools", "tool"), new("Tinker's tools", "tool"),
            ],
        };

    public static readonly Dictionary<string, List<(int Level, int Slots1, int Slots2, int Slots3, int Slots4, int Slots5, int Slots6, int Slots7, int Slots8, int Slots9)>>
        SpellSlotTable = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Bard"] =
            [
                (1, 2,0,0,0,0,0,0,0,0), (2, 3,0,0,0,0,0,0,0,0), (3, 4,2,0,0,0,0,0,0,0),
                (4, 4,3,0,0,0,0,0,0,0), (5, 4,3,2,0,0,0,0,0,0), (6, 4,3,3,0,0,0,0,0,0),
                (7, 4,3,3,1,0,0,0,0,0), (8, 4,3,3,2,0,0,0,0,0), (9, 4,3,3,3,1,0,0,0,0),
                (10,4,3,3,3,2,0,0,0,0),(11,4,3,3,3,2,1,0,0,0),(12,4,3,3,3,2,1,0,0,0),
                (13,4,3,3,3,2,1,1,0,0),(14,4,3,3,3,2,1,1,0,0),(15,4,3,3,3,2,1,1,1,0),
                (16,4,3,3,3,2,1,1,1,0),(17,4,3,3,3,2,1,1,1,1),(18,4,3,3,3,3,1,1,1,1),
                (19,4,3,3,3,3,2,1,1,1),(20,4,3,3,3,3,2,2,1,1),
            ],
            ["Cleric"] =
            [
                (1, 2,0,0,0,0,0,0,0,0), (2, 3,0,0,0,0,0,0,0,0), (3, 4,2,0,0,0,0,0,0,0),
                (4, 4,3,0,0,0,0,0,0,0), (5, 4,3,2,0,0,0,0,0,0), (6, 4,3,3,0,0,0,0,0,0),
                (7, 4,3,3,1,0,0,0,0,0), (8, 4,3,3,2,0,0,0,0,0), (9, 4,3,3,3,1,0,0,0,0),
                (10,4,3,3,3,2,0,0,0,0),(11,4,3,3,3,2,1,0,0,0),(12,4,3,3,3,2,1,0,0,0),
                (13,4,3,3,3,2,1,1,0,0),(14,4,3,3,3,2,1,1,0,0),(15,4,3,3,3,2,1,1,1,0),
                (16,4,3,3,3,2,1,1,1,0),(17,4,3,3,3,2,1,1,1,1),(18,4,3,3,3,3,1,1,1,1),
                (19,4,3,3,3,3,2,1,1,1),(20,4,3,3,3,3,2,2,1,1),
            ],
            ["Druid"] =
            [
                (1, 2,0,0,0,0,0,0,0,0), (2, 3,0,0,0,0,0,0,0,0), (3, 4,2,0,0,0,0,0,0,0),
                (4, 4,3,0,0,0,0,0,0,0), (5, 4,3,2,0,0,0,0,0,0), (6, 4,3,3,0,0,0,0,0,0),
                (7, 4,3,3,1,0,0,0,0,0), (8, 4,3,3,2,0,0,0,0,0), (9, 4,3,3,3,1,0,0,0,0),
                (10,4,3,3,3,2,0,0,0,0),(11,4,3,3,3,2,1,0,0,0),(12,4,3,3,3,2,1,0,0,0),
                (13,4,3,3,3,2,1,1,0,0),(14,4,3,3,3,2,1,1,0,0),(15,4,3,3,3,2,1,1,1,0),
                (16,4,3,3,3,2,1,1,1,0),(17,4,3,3,3,2,1,1,1,1),(18,4,3,3,3,3,1,1,1,1),
                (19,4,3,3,3,3,2,1,1,1),(20,4,3,3,3,3,2,2,1,1),
            ],
            ["Sorcerer"] =
            [
                (1, 2,0,0,0,0,0,0,0,0), (2, 3,0,0,0,0,0,0,0,0), (3, 4,2,0,0,0,0,0,0,0),
                (4, 4,3,0,0,0,0,0,0,0), (5, 4,3,2,0,0,0,0,0,0), (6, 4,3,3,0,0,0,0,0,0),
                (7, 4,3,3,1,0,0,0,0,0), (8, 4,3,3,2,0,0,0,0,0), (9, 4,3,3,3,1,0,0,0,0),
                (10,4,3,3,3,2,0,0,0,0),(11,4,3,3,3,2,1,0,0,0),(12,4,3,3,3,2,1,0,0,0),
                (13,4,3,3,3,2,1,1,0,0),(14,4,3,3,3,2,1,1,0,0),(15,4,3,3,3,2,1,1,1,0),
                (16,4,3,3,3,2,1,1,1,0),(17,4,3,3,3,2,1,1,1,1),(18,4,3,3,3,3,1,1,1,1),
                (19,4,3,3,3,3,2,1,1,1),(20,4,3,3,3,3,2,2,1,1),
            ],
            ["Wizard"] =
            [
                (1, 2,0,0,0,0,0,0,0,0), (2, 3,0,0,0,0,0,0,0,0), (3, 4,2,0,0,0,0,0,0,0),
                (4, 4,3,0,0,0,0,0,0,0), (5, 4,3,2,0,0,0,0,0,0), (6, 4,3,3,0,0,0,0,0,0),
                (7, 4,3,3,1,0,0,0,0,0), (8, 4,3,3,2,0,0,0,0,0), (9, 4,3,3,3,1,0,0,0,0),
                (10,4,3,3,3,2,0,0,0,0),(11,4,3,3,3,2,1,0,0,0),(12,4,3,3,3,2,1,0,0,0),
                (13,4,3,3,3,2,1,1,0,0),(14,4,3,3,3,2,1,1,0,0),(15,4,3,3,3,2,1,1,1,0),
                (16,4,3,3,3,2,1,1,1,0),(17,4,3,3,3,2,1,1,1,1),(18,4,3,3,3,3,1,1,1,1),
                (19,4,3,3,3,3,2,1,1,1),(20,4,3,3,3,3,2,2,1,1),
            ],
            ["Warlock"] =
            [
                (1, 1,0,0,0,0,0,0,0,0),(2, 2,0,0,0,0,0,0,0,0),(3, 0,2,0,0,0,0,0,0,0),
                (4, 0,2,0,0,0,0,0,0,0),(5, 0,0,2,0,0,0,0,0,0),(6, 0,0,2,0,0,0,0,0,0),
                (7, 0,0,0,2,0,0,0,0,0),(8, 0,0,0,2,0,0,0,0,0),(9, 0,0,0,0,2,0,0,0,0),
                (10,0,0,0,0,2,0,0,0,0),(11,0,0,0,0,3,0,0,0,0),(12,0,0,0,0,3,0,0,0,0),
                (13,0,0,0,0,3,0,0,0,0),(14,0,0,0,0,3,0,0,0,0),(15,0,0,0,0,3,0,0,0,0),
                (16,0,0,0,0,3,0,0,0,0),(17,0,0,0,0,4,0,0,0,0),(18,0,0,0,0,4,0,0,0,0),
                (19,0,0,0,0,4,0,0,0,0),(20,0,0,0,0,4,0,0,0,0),
            ],
            ["Paladin"] =
            [
                (1, 0,0,0,0,0,0,0,0,0),(2, 2,0,0,0,0,0,0,0,0),(3, 3,0,0,0,0,0,0,0,0),
                (4, 3,0,0,0,0,0,0,0,0),(5, 4,2,0,0,0,0,0,0,0),(6, 4,2,0,0,0,0,0,0,0),
                (7, 4,3,0,0,0,0,0,0,0),(8, 4,3,0,0,0,0,0,0,0),(9, 4,3,2,0,0,0,0,0,0),
                (10,4,3,2,0,0,0,0,0,0),(11,4,3,3,0,0,0,0,0,0),(12,4,3,3,0,0,0,0,0,0),
                (13,4,3,3,1,0,0,0,0,0),(14,4,3,3,1,0,0,0,0,0),(15,4,3,3,2,0,0,0,0,0),
                (16,4,3,3,2,0,0,0,0,0),(17,4,3,3,3,1,0,0,0,0),(18,4,3,3,3,1,0,0,0,0),
                (19,4,3,3,3,2,0,0,0,0),(20,4,3,3,3,2,0,0,0,0),
            ],
            ["Ranger"] =
            [
                (1, 0,0,0,0,0,0,0,0,0),(2, 2,0,0,0,0,0,0,0,0),(3, 3,0,0,0,0,0,0,0,0),
                (4, 3,0,0,0,0,0,0,0,0),(5, 4,2,0,0,0,0,0,0,0),(6, 4,2,0,0,0,0,0,0,0),
                (7, 4,3,0,0,0,0,0,0,0),(8, 4,3,0,0,0,0,0,0,0),(9, 4,3,2,0,0,0,0,0,0),
                (10,4,3,2,0,0,0,0,0,0),(11,4,3,3,0,0,0,0,0,0),(12,4,3,3,0,0,0,0,0,0),
                (13,4,3,3,1,0,0,0,0,0),(14,4,3,3,1,0,0,0,0,0),(15,4,3,3,2,0,0,0,0,0),
                (16,4,3,3,2,0,0,0,0,0),(17,4,3,3,3,1,0,0,0,0),(18,4,3,3,3,1,0,0,0,0),
                (19,4,3,3,3,2,0,0,0,0),(20,4,3,3,3,2,0,0,0,0),
            ],
            ["Artificer"] =
            [
                (1, 0,0,0,0,0,0,0,0,0),(2, 2,0,0,0,0,0,0,0,0),(3, 3,0,0,0,0,0,0,0,0),
                (4, 3,0,0,0,0,0,0,0,0),(5, 4,2,0,0,0,0,0,0,0),(6, 4,2,0,0,0,0,0,0,0),
                (7, 4,3,0,0,0,0,0,0,0),(8, 4,3,0,0,0,0,0,0,0),(9, 4,3,2,0,0,0,0,0,0),
                (10,4,3,2,0,0,0,0,0,0),(11,4,3,3,0,0,0,0,0,0),(12,4,3,3,0,0,0,0,0,0),
                (13,4,3,3,1,0,0,0,0,0),(14,4,3,3,1,0,0,0,0,0),(15,4,3,3,2,0,0,0,0,0),
                (16,4,3,3,2,0,0,0,0,0),(17,4,3,3,3,1,0,0,0,0),(18,4,3,3,3,1,0,0,0,0),
                (19,4,3,3,3,2,0,0,0,0),(20,4,3,3,3,2,0,0,0,0),
            ],
        };

    public static List<int[]> GetSpellSlots(string className, int level)
    {
        if (!SpellSlotTable.TryGetValue(className, out var table))
            return [];

        var row = table.FirstOrDefault(r => r.Level == level);
        if (row == default) return [];

        int[] raw = [row.Slots1, row.Slots2, row.Slots3, row.Slots4,
                     row.Slots5, row.Slots6, row.Slots7, row.Slots8, row.Slots9];

        var result = new List<int[]>();
        for (int i = 0; i < raw.Length; i++)
            if (raw[i] > 0)
                result.Add([i + 1, raw[i]]);

        return result;
    }

    public static List<FeatureEntry> GetClassFeaturesUpToLevel(string className, int level)
    {
        if (!ClassFeaturesByLevel.TryGetValue(className, out var features))
            return [];
        return features.Where(f => f.Level <= level).ToList();
    }

    public static List<FeatureEntry> GetClassFeaturesAtLevel(string className, int level)
    {
        if (!ClassFeaturesByLevel.TryGetValue(className, out var features))
            return [];
        return features.Where(f => f.Level == level).ToList();
    }

    public static List<FeatureEntry> GetRaceFeatures(string race) =>
        RaceFeatures.TryGetValue(race, out var features) ? features : [];

    public static List<ProficiencyEntry> GetClassProficiencies(string className) =>
        ClassDefaultProficiencies.TryGetValue(className, out var profs) ? profs : [];

    public static string? GetSpellAbility(string className) =>
        ClassSpellAbility.TryGetValue(className, out var ability) ? ability : null;

    public static readonly Dictionary<string, string> ClassHitDice = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Barbarian"] = "d12",
        ["Fighter"]   = "d10",
        ["Paladin"]   = "d10",
        ["Ranger"]    = "d10",
        ["Bard"]      = "d8",
        ["Cleric"]    = "d8",
        ["Druid"]     = "d8",
        ["Monk"]      = "d8",
        ["Rogue"]     = "d8",
        ["Warlock"]   = "d8",
        ["Artificer"] = "d8",
        ["Sorcerer"]  = "d6",
        ["Wizard"]    = "d6",
    };

    public static readonly Dictionary<string, string[]> ClassSavingThrows = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Barbarian"] = ["str", "con"],
        ["Bard"]      = ["dex", "cha"],
        ["Cleric"]    = ["wis", "cha"],
        ["Druid"]     = ["int", "wis"],
        ["Fighter"]   = ["str", "con"],
        ["Monk"]      = ["str", "dex"],
        ["Paladin"]   = ["wis", "cha"],
        ["Ranger"]    = ["str", "dex"],
        ["Rogue"]     = ["dex", "int"],
        ["Sorcerer"]  = ["con", "cha"],
        ["Warlock"]   = ["wis", "cha"],
        ["Wizard"]    = ["int", "wis"],
        ["Artificer"] = ["con", "int"],
    };

    public static readonly Dictionary<string, int> ClassSkillCount = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Bard"]   = 3,
        ["Ranger"] = 3,
        ["Rogue"]  = 4,
    };

    public static string GetHitDice(string className) =>
        ClassHitDice.TryGetValue(className, out var hd) ? hd : "d8";

    public static string[] GetSavingThrows(string className) =>
        ClassSavingThrows.TryGetValue(className, out var st) ? st : [];

    public static int GetSkillCount(string className) =>
        ClassSkillCount.TryGetValue(className, out var sc) ? sc : 2;
}
