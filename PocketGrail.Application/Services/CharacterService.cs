namespace PocketGrail.Application.Services;

using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Mappers;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Characters;
using PocketGrail.Domain.Entities.ClassEntities;
using PocketGrail.Domain.Entities.Enums;
using PocketGrail.Domain.Entities.Proficiencies;

public sealed class CharacterService : ICharacterService
{
    private readonly ICharacterRepository _repository;
    private readonly IItemRepository _itemRepository;
    private readonly ISpellRepository _spellRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IClassRepository _classRepository;
    private readonly IRaceRepository _raceRepository;

    public CharacterService(
        ICharacterRepository repository,
        IItemRepository itemRepository,
        ISpellRepository spellRepository,
        ICloudinaryService cloudinaryService,
        IClassRepository classRepository,
        IRaceRepository raceRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
        _spellRepository = spellRepository;
        _cloudinaryService = cloudinaryService;
        _classRepository = classRepository;
        _raceRepository = raceRepository;
    }

    // ── Basic queries ──────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<CharacterDto>> GetMyCharactersAsync(int userId, CancellationToken ct = default)
    {
        var characters = await _repository.GetByOwnerIdAsync(userId, ct);
        return characters.Select(CharacterMapper.ToDto).ToList();
    }

    public async Task<CharacterDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var character = await _repository.GetByIdAsync(id, ct);
        return character is null ? null : CharacterMapper.ToDto(character);
    }

    public async Task<CharacterDetailDto?> GetCharacterDetailAsync(int id, int userId, CancellationToken ct = default)
    {
        var character = await _repository.GetDetailByIdAsync(id, ct);
        if (character is null) return null;
        if (character.OwnerId != userId) throw new UnauthorizedAccessException("Access denied.");
        return CharacterMapper.ToDetailDto(character);
    }

    // ── Create / Update / Delete ───────────────────────────────────────────────

    public async Task<CharacterDto> CreateCharacterAsync(
        CreateCharacterRequest request, int userId, CancellationToken ct = default)
    {
        var cls = await _classRepository.GetByNameWithDetailsAsync(request.ClassName, ct)
            ?? throw new KeyNotFoundException($"Class '{request.ClassName}' not found.");
        var race = await _raceRepository.GetByNameWithDetailsAsync(request.Race, ct)
            ?? throw new KeyNotFoundException($"Race '{request.Race}' not found.");

        var startLevel = Math.Max(1, request.StartLevel);

        if (race.FlexibleBonusPoints > 0)
        {
            var total = request.FlexStrBonus + request.FlexDexBonus + request.FlexConBonus
                      + request.FlexIntBonus + request.FlexWisBonus + request.FlexChaBonus;
            if (total != race.FlexibleBonusPoints)
                throw new InvalidOperationException(
                    $"Must distribute exactly {race.FlexibleBonusPoints} flexible bonus points. Got {total}.");
        }

        if (cls.SkillChoiceCount > 0 && cls.AvailableSkillChoices.Count > 0 && request.SkillChoices.Count != cls.SkillChoiceCount)
            throw new InvalidOperationException(
                $"Must choose exactly {cls.SkillChoiceCount} skills for {cls.Name}. Got {request.SkillChoices.Count}.");

        Subclass? subclass = null;
        if (request.SubclassId.HasValue)
        {
            subclass = await _classRepository.GetSubclassByIdAsync(request.SubclassId.Value, ct)
                ?? throw new KeyNotFoundException($"Subclass {request.SubclassId} not found.");
            if (subclass.ClassId != cls.Id)
                throw new InvalidOperationException(
                    $"Subclass '{subclass.Name}' does not belong to {cls.Name}.");
        }

        string? imageUrl = null;
        if (request.Image is not null)
            imageUrl = await _cloudinaryService.UploadImageAsync(request.Image, ct: ct);

        var now = DateTime.UtcNow;

        var stats = new CharacterStats
        {
            Strength     = request.StrScore + race.StrBonus + request.FlexStrBonus,
            Dexterity    = request.DexScore + race.DexBonus + request.FlexDexBonus,
            Constitution = request.ConScore + race.ConBonus + request.FlexConBonus,
            Intelligence = request.IntScore + race.IntBonus + request.FlexIntBonus,
            Wisdom       = request.WisScore + race.WisBonus + request.FlexWisBonus,
            Charisma     = request.ChaScore + race.ChaBonus + request.FlexChaBonus,
            CreatedAt = now,
            UpdatedAt = now
        };

        var profs = new CharacterProficiencies { CreatedAt = now, UpdatedAt = now };
        ApplyRaceProficiencies(profs, race, now);
        ApplyClassLevel1Proficiencies(profs, cls, now);
        ApplyPlayerChoices(profs, request, now);

        var character = new Character
        {
            Name       = request.Name,
            Race       = request.Race,
            Level      = startLevel,
            CurrentHp  = 0,
            MaxHp      = 0,
            Speed      = race.BaseSpeed,
            OwnerId    = userId,
            CampaignId = request.CampaignId,
            ImageUrl   = imageUrl,
            CreatedAt  = now,
            UpdatedAt  = now,
            Wallet         = new CharacterWallet { CreatedAt = now, UpdatedAt = now },
            CharacterStats = stats,
            Proficiencies  = profs
        };

        character.Classes.Add(new CharacterClass
        {
            ClassId             = cls.Id,
            ClassLevel          = startLevel,
            TotalHitDiceCount   = startLevel,
            CharacterSubclassId = subclass?.Id,
            CreatedAt           = now,
            UpdatedAt           = now
        });

        foreach (var rf in race.Features)
            character.Features.Add(new Feature { Name = rf.Name, Description = rf.Description, CreatedAt = now, UpdatedAt = now });

        // Apply all base class features for levels 1 through startLevel
        foreach (var cf in cls.ClassFeatures.Where(cf =>
            cf.GainingLevel <= startLevel
            && cf.Name != "Starting Proficiencies"))
            character.Features.Add(new Feature { Name = cf.Name, Description = cf.Description, CreatedAt = now, UpdatedAt = now });

        // Apply subclass features for levels 1 through startLevel
        if (subclass is not null)
        {
            foreach (var sf in subclass.SubclassFeatures.Where(sf => sf.GainingLevel <= startLevel))
                character.Features.Add(new Feature { Name = sf.Name, Description = sf.Description, CreatedAt = now, UpdatedAt = now });
        }

        // Apply spell slots for all levels up to startLevel
        for (var level = 1; level <= startLevel; level++)
        {
            foreach (var t in cls.SpellSlotTemplates.Where(t => t.ClassLevel == level && t.TotalSlots > 0))
            {
                var existing = character.SpellSlots.FirstOrDefault(s => s.SlotLevel == t.SpellSlotLevel);
                if (existing is null)
                    character.SpellSlots.Add(new SpellSlot
                        { SlotLevel = t.SpellSlotLevel, TotalSlots = t.TotalSlots, RemainingSlots = t.TotalSlots, CreatedAt = now, UpdatedAt = now });
                else if (t.TotalSlots > existing.TotalSlots)
                {
                    existing.TotalSlots     = t.TotalSlots;
                    existing.RemainingSlots = t.TotalSlots;
                }
            }
        }

        await _repository.AddAsync(character, ct);
        await _repository.SaveChangesAsync(ct);

        if (request.StartingItemIds.Count > 0)
        {
            var withItems = await _repository.GetDetailByIdAsync(character.Id, ct)
                            ?? throw new InvalidOperationException("Character not found after creation.");
            var items = await _repository.GetItemsByIdsAsync(request.StartingItemIds, ct);
            foreach (var item in items)
                withItems.Items.Add(item);
            await _repository.SaveChangesAsync(ct);
        }

        var created = await _repository.GetByIdAsync(character.Id, ct)
                      ?? throw new InvalidOperationException("Failed to retrieve created character.");
        return CharacterMapper.ToDto(created);
    }

    public async Task<CharacterDto> UpdateCharacterAsync(
        int id, UpdateCharacterRequest request, int userId, CancellationToken ct = default)
    {
        var character = await _repository.GetDetailByIdAsync(id, ct)
                        ?? throw new KeyNotFoundException("Character not found.");
        if (character.OwnerId != userId)
            throw new UnauthorizedAccessException("Only the character owner can update it.");

        if (request.Name is not null) character.Name = request.Name;
        if (request.Race is not null) character.Race = request.Race;
        if (request.CurrentHp.HasValue) character.CurrentHp = request.CurrentHp.Value;
        if (request.MaxHp.HasValue) character.MaxHp = request.MaxHp.Value;
        if (request.CampaignId.HasValue) character.CampaignId = request.CampaignId.Value;
        if (request.Alignment is not null) character.Alignment = request.Alignment;
        if (request.BackgroundStory is not null) character.BackgroundStory = request.BackgroundStory;
        if (request.Appearance is not null) character.Appearance = request.Appearance;
        if (request.Notes is not null) character.Notes = request.Notes;

        if (request.Image is not null)
            character.ImageUrl = await _cloudinaryService.UploadImageAsync(request.Image, ct: ct);

        character.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);

        return CharacterMapper.ToDto(character);
    }

    public async Task DeleteCharacterAsync(int id, int userId, CancellationToken ct = default)
    {
        var character = await _repository.GetByIdAsync(id, ct)
                        ?? throw new KeyNotFoundException("Character not found.");
        if (character.OwnerId != userId)
            throw new UnauthorizedAccessException("Only the character owner can delete it.");
        await _repository.DeleteAsync(character, ct);
        await _repository.SaveChangesAsync(ct);
    }

    // ── Stats / Vitals / Wallet / Image ───────────────────────────────────────

    public async Task<CharacterDetailDto> UpdateStatsAsync(int id, UpdateStatsRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(id, userId, ct);
        var now = DateTime.UtcNow;
        if (c.CharacterStats is null)
            c.CharacterStats = new CharacterStats { CharacterId = c.Id, CreatedAt = now, UpdatedAt = now };

        if (request.StrScore.HasValue) c.CharacterStats.Strength = request.StrScore.Value;
        if (request.DexScore.HasValue) c.CharacterStats.Dexterity = request.DexScore.Value;
        if (request.ConScore.HasValue) c.CharacterStats.Constitution = request.ConScore.Value;
        if (request.IntScore.HasValue) c.CharacterStats.Intelligence = request.IntScore.Value;
        if (request.WisScore.HasValue) c.CharacterStats.Wisdom = request.WisScore.Value;
        if (request.ChaScore.HasValue) c.CharacterStats.Charisma = request.ChaScore.Value;
        if (request.ArmorClass.HasValue) c.ArmorClass = request.ArmorClass.Value;
        if (request.Speed.HasValue) c.Speed = request.Speed.Value;
        if (request.Alignment is not null) c.Alignment = request.Alignment;
        c.CharacterStats.UpdatedAt = now;
        c.UpdatedAt = now;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToDetailDto(c);
    }

    public async Task<CharacterDetailDto> UpdateVitalsAsync(int id, UpdateVitalsRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(id, userId, ct);
        if (request.CurrentHp.HasValue) c.CurrentHp = request.CurrentHp.Value;
        if (request.MaxHp.HasValue) c.MaxHp = request.MaxHp.Value;
        if (request.TempHp.HasValue) c.TempHp = request.TempHp.Value;
        if (request.XpPoints.HasValue) c.XpPoints = request.XpPoints.Value;
        if (request.HasInspiration.HasValue) c.HasInspiration = request.HasInspiration.Value;
        if (request.Exhaustion.HasValue) c.Exhaustion = request.Exhaustion.Value;
        if (request.DeathSuccesses.HasValue) c.DeathSuccesses = request.DeathSuccesses.Value;
        if (request.DeathFailures.HasValue) c.DeathFailures = request.DeathFailures.Value;
        c.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToDetailDto(c);
    }

    public async Task<CharacterDetailDto> UpdateWalletAsync(int id, UpdateWalletRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(id, userId, ct);
        var wallet = c.Wallet ?? new CharacterWallet
            { CharacterId = c.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        if (request.CpCoins.HasValue) wallet.CpCoins = request.CpCoins.Value;
        if (request.SpCoins.HasValue) wallet.SpCoins = request.SpCoins.Value;
        if (request.EpCoins.HasValue) wallet.EpCoins = request.EpCoins.Value;
        if (request.GpCoins.HasValue) wallet.GpCoins = request.GpCoins.Value;
        if (request.PpCoins.HasValue) wallet.PpCoins = request.PpCoins.Value;
        wallet.UpdatedAt = DateTime.UtcNow;
        c.Wallet = wallet;
        c.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToDetailDto(c);
    }

    public async Task<CharacterDetailDto> UpdateImageAsync(int id, UpdateCharacterImageRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(id, userId, ct);
        var folder = $"pocket-grail/characters/{id}";
        c.ImageUrl = await _cloudinaryService.UploadImageAsync(request.Image, folder, ct);
        c.ImageCropX = request.CropX;
        c.ImageCropY = request.CropY;
        c.ImageCropWidth = request.CropWidth;
        c.ImageCropHeight = request.CropHeight;
        c.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToDetailDto(c);
    }

    // ── Items ──────────────────────────────────────────────────────────────────

    public async Task<ItemDto> AddItemAsync(int characterId, AddItemRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var item = new Item
        {
            Name = request.Name, Description = request.Description, Rarity = request.Rarity,
            Category = request.Category, Weight = request.Weight, Cost = request.Cost,
            IsWeapon = request.IsWeapon, IsMagical = request.IsMagical, AtkMod = request.AtkMod,
            Damage = request.Damage, DamageType = request.DamageType,
            WeaponProperties = request.WeaponProperties, ChargesInfo = request.ChargesInfo,
            RechargeType = request.RechargeType, Tags = request.Tags,
            CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
        };
        c.Items.Add(item);
        await _repository.SaveChangesAsync(ct);

        var junction = item.CharacterItems.FirstOrDefault(ci => ci.CharacterId == characterId);
        if (junction is not null)
        {
            junction.IsEquipped = request.IsEquipped;
            junction.IsAttuned = request.IsAttuned;
            junction.Quantity = request.Quantity;
            await _repository.SaveChangesAsync(ct);
        }

        return CharacterMapper.ToItemDto(item, request.IsEquipped, request.IsAttuned, request.Quantity);
    }

    public async Task<ItemDto> AddItemFromCatalogAsync(int characterId, int itemId, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);

        if (c.Items.Any(i => i.Id == itemId))
            throw new InvalidOperationException("This item is already in the character's inventory.");

        var catalogItem = await _itemRepository.GetByIdAsync(itemId, ct)
            ?? throw new KeyNotFoundException("Catalog item not found.");

        await _repository.LinkItemAsync(characterId, itemId, ct);
        await _repository.SaveChangesAsync(ct);

        return CharacterMapper.ToItemDto(catalogItem, false, false, 1);
    }

    public async Task<ItemDto> UpdateItemAsync(int characterId, int itemId, UpdateItemRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var item = c.Items.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new KeyNotFoundException("Item not found on this character.");
        var junction = item.CharacterItems.FirstOrDefault(ci => ci.CharacterId == characterId)
                       ?? throw new KeyNotFoundException("Item junction not found on this character.");
        if (request.IsEquipped.HasValue) junction.IsEquipped = request.IsEquipped.Value;
        if (request.IsAttuned.HasValue) junction.IsAttuned = request.IsAttuned.Value;
        if (request.Quantity.HasValue) junction.Quantity = request.Quantity.Value;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToItemDto(item, junction.IsEquipped, junction.IsAttuned, junction.Quantity);
    }

    public async Task DeleteItemAsync(int characterId, int itemId, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var item = c.Items.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new KeyNotFoundException("Item not found on this character.");
        c.Items.Remove(item);
        await _repository.SaveChangesAsync(ct);
    }

    // ── Spells ─────────────────────────────────────────────────────────────────

    public async Task<SpellDto> AddSpellAsync(int characterId, AddSpellRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var spell = new Spell
        {
            Name = request.Name, Level = request.Level, School = request.School, Range = request.Range,
            CastingTime = request.CastingTime, Concentration = request.Concentration,
            IsRitual = request.IsRitual, Components = request.Components,
            CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
        };
        c.Spells.Add(spell);
        await _repository.SaveChangesAsync(ct);

        var junction = spell.CharacterSpells.FirstOrDefault(cs => cs.CharacterId == characterId);
        if (junction is not null)
        {
            junction.Prepared = request.Prepared;
            await _repository.SaveChangesAsync(ct);
        }

        return CharacterMapper.ToSpellDto(spell, request.Prepared);
    }

    public async Task<SpellDto> AddSpellFromCatalogAsync(int characterId, int spellId, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);

        if (c.Spells.Any(s => s.Id == spellId))
            throw new InvalidOperationException("This spell is already in the character's spellbook.");

        var catalogSpell = await _spellRepository.GetByIdAsync(spellId, ct)
            ?? throw new KeyNotFoundException("Catalog spell not found.");

        await _repository.LinkSpellAsync(characterId, spellId, ct);
        await _repository.SaveChangesAsync(ct);

        return CharacterMapper.ToSpellDto(catalogSpell, false);
    }

    public async Task<SpellDto> ToggleSpellPreparedAsync(int characterId, int spellId, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var spell = c.Spells.FirstOrDefault(s => s.Id == spellId)
                    ?? throw new KeyNotFoundException("Spell not found on this character.");
        var junction = spell.CharacterSpells.FirstOrDefault(cs => cs.CharacterId == characterId)
                       ?? throw new KeyNotFoundException("Spell junction not found on this character.");
        junction.Prepared = !junction.Prepared;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToSpellDto(spell, junction.Prepared);
    }

    public async Task DeleteSpellAsync(int characterId, int spellId, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var spell = c.Spells.FirstOrDefault(s => s.Id == spellId)
                    ?? throw new KeyNotFoundException("Spell not found on this character.");
        c.Spells.Remove(spell);
        await _repository.SaveChangesAsync(ct);
    }

    public async Task<SpellSlotDto> UpdateSpellSlotAsync(int characterId, UpdateSpellSlotRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var slot = c.SpellSlots.FirstOrDefault(s => s.SlotLevel == request.SlotLevel)
                   ?? throw new KeyNotFoundException($"No spell slot of level {request.SlotLevel} found.");
        slot.RemainingSlots = Math.Clamp(request.RemainingSlots, 0, slot.TotalSlots);
        await _repository.SaveChangesAsync(ct);
        return new SpellSlotDto
            { Id = slot.Id, SlotLevel = slot.SlotLevel, TotalSlots = slot.TotalSlots, RemainingSlots = slot.RemainingSlots };
    }

    // ── Feats ──────────────────────────────────────────────────────────────────

    public async Task<FeatDto> AddFeatAsync(int characterId, AddFeatRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var feat = new Feat
        {
            Name = request.Name, Requirement = request.Requirement, Description = request.Description,
            CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
        };
        c.Feats.Add(feat);
        await _repository.SaveChangesAsync(ct);
        return new FeatDto { Id = feat.Id, Name = feat.Name, Requirement = feat.Requirement, Description = feat.Description };
    }

    public async Task DeleteFeatAsync(int characterId, int featId, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var feat = c.Feats.FirstOrDefault(f => f.Id == featId)
                   ?? throw new KeyNotFoundException("Feat not found on this character.");
        c.Feats.Remove(feat);
        await _repository.SaveChangesAsync(ct);
    }

    // ── Features ───────────────────────────────────────────────────────────────

    public async Task<FeatureDto> AddFeatureAsync(int characterId, AddFeatureRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var feature = new Feature
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        c.Features.Add(feature);
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToFeatureDto(feature);
    }

    public async Task DeleteFeatureAsync(int characterId, int featureId, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var feature = c.Features.FirstOrDefault(f => f.Id == featureId)
                      ?? throw new KeyNotFoundException("Feature not found on this character.");
        c.Features.Remove(feature);
        await _repository.SaveChangesAsync(ct);
    }

    // ── Proficiencies ──────────────────────────────────────────────────────────

    public async Task<ProficiencyDto> AddProficiencyAsync(int characterId, AddProficiencyRequest request, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var now = DateTime.UtcNow;

        if (c.Proficiencies is null)
            c.Proficiencies = new CharacterProficiencies { CharacterId = c.Id, CreatedAt = now, UpdatedAt = now };

        int newId;

        switch (request.ProficiencyType.ToLowerInvariant())
        {
            case "skill":
                if (!Enum.TryParse<Skill>(request.Name, true, out var skill))
                    throw new InvalidOperationException($"Unknown skill: {request.Name}.");
                var sp = new SkillProficiency
                    { Skill = skill, HasExpertise = request.HasExpertise, CharacterProficienciesId = c.Proficiencies.Id, CreatedAt = now, UpdatedAt = now };
                c.Proficiencies.Skills.Add(sp);
                await _repository.SaveChangesAsync(ct);
                newId = sp.Id;
                break;

            case "weapon":
                var wp = new WeaponProficiency { Name = request.Name, CreatedAt = now, UpdatedAt = now };
                c.Proficiencies.Weapons.Add(wp);
                await _repository.SaveChangesAsync(ct);
                newId = wp.Id;
                break;

            case "armor":
                var ap = new ArmorProficiency { Name = request.Name, CreatedAt = now, UpdatedAt = now };
                c.Proficiencies.Armors.Add(ap);
                await _repository.SaveChangesAsync(ct);
                newId = ap.Id;
                break;

            case "language":
                var lg = new Language { Name = request.Name, CreatedAt = now, UpdatedAt = now };
                c.Proficiencies.Languages.Add(lg);
                await _repository.SaveChangesAsync(ct);
                newId = lg.Id;
                break;

            case "instrument":
                var ig = new Instrument { Name = request.Name, CreatedAt = now, UpdatedAt = now };
                c.Proficiencies.Instruments.Add(ig);
                await _repository.SaveChangesAsync(ct);
                newId = ig.Id;
                break;

            case "savingthrow":
                if (!Enum.TryParse<Ability>(request.Name, true, out var ability))
                    throw new InvalidOperationException($"Unknown ability: {request.Name}.");
                var st = new AdditionalSavingThrowProficiency
                    { Ability = ability, CharacterProficienciesId = c.Proficiencies.Id, CreatedAt = now, UpdatedAt = now };
                c.Proficiencies.AdditionalSavingThrows.Add(st);
                await _repository.SaveChangesAsync(ct);
                newId = st.Id;
                break;

            default:
                throw new InvalidOperationException($"Unknown proficiency type: '{request.ProficiencyType}'. Valid: skill, weapon, armor, language, instrument, savingthrow.");
        }

        return new ProficiencyDto
            { Id = newId, Name = request.Name, ProficiencyType = request.ProficiencyType, HasExpertise = request.HasExpertise, AbilityKey = request.AbilityKey };
    }

    public async Task DeleteProficiencyAsync(int characterId, int proficiencyId, int userId,
        CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        if (c.Proficiencies is null) return;

        var skill = c.Proficiencies.Skills.FirstOrDefault(s => s.Id == proficiencyId);
        if (skill is not null) { c.Proficiencies.Skills.Remove(skill); await _repository.SaveChangesAsync(ct); return; }

        var weapon = c.Proficiencies.Weapons.FirstOrDefault(w => w.Id == proficiencyId);
        if (weapon is not null) { c.Proficiencies.Weapons.Remove(weapon); await _repository.SaveChangesAsync(ct); return; }

        var armor = c.Proficiencies.Armors.FirstOrDefault(a => a.Id == proficiencyId);
        if (armor is not null) { c.Proficiencies.Armors.Remove(armor); await _repository.SaveChangesAsync(ct); return; }

        var language = c.Proficiencies.Languages.FirstOrDefault(l => l.Id == proficiencyId);
        if (language is not null) { c.Proficiencies.Languages.Remove(language); await _repository.SaveChangesAsync(ct); return; }

        var instrument = c.Proficiencies.Instruments.FirstOrDefault(i => i.Id == proficiencyId);
        if (instrument is not null) { c.Proficiencies.Instruments.Remove(instrument); await _repository.SaveChangesAsync(ct); return; }

        var savingThrow = c.Proficiencies.AdditionalSavingThrows.FirstOrDefault(s => s.Id == proficiencyId);
        if (savingThrow is not null) { c.Proficiencies.AdditionalSavingThrows.Remove(savingThrow); await _repository.SaveChangesAsync(ct); return; }
    }

    // ── Allies ─────────────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<AllyDto>> GetAlliesAsync(int characterId, int userId,
        CancellationToken ct = default)
    {
        var character = await _repository.GetByIdAsync(characterId, ct)
                        ?? throw new KeyNotFoundException("Character not found.");
        if (character.OwnerId != userId) throw new UnauthorizedAccessException("Access denied.");
        if (character.CampaignId is null) return [];

        var campaignChars = await _repository.GetCampaignCharactersAsync(character.CampaignId.Value, ct);
        return campaignChars
            .Where(c => c.Id != characterId)
            .Select(c => new AllyDto
            {
                CharacterId   = c.Id,
                CharacterName = c.Name,
                Race          = c.Race,
                Classes       = c.Classes.Select(CharacterMapper.ToClassDto).ToList(),
                ClassDisplay  = CharacterMapper.FormatClassDisplay(c.Classes),
                Level         = c.Level,
                CurrentHp     = c.CurrentHp,
                MaxHp         = c.MaxHp,
                ImageUrl      = c.ImageUrl,
                ImageCropX    = c.ImageCropX,
                ImageCropY    = c.ImageCropY,
                ImageCropWidth  = c.ImageCropWidth,
                ImageCropHeight = c.ImageCropHeight,
                UserId   = c.OwnerId,
                Username = c.Owner?.Username ?? string.Empty
            })
            .ToList();
    }

    // ── Character class management ─────────────────────────────────────────────

    public async Task<CharacterClassDto> AddCharacterClassAsync(
        int characterId, AddCharacterClassRequest request, int userId, CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);

        if (character.Classes.Any(cc => cc.Class?.Name.Equals(request.ClassName, StringComparison.OrdinalIgnoreCase) == true))
            throw new InvalidOperationException($"Character already has a level in {request.ClassName}.");

        var cls = await _classRepository.GetByNameWithDetailsAsync(request.ClassName, ct)
            ?? throw new KeyNotFoundException($"Class '{request.ClassName}' not found.");

        if (character.Classes.Count > 0)
        {
            foreach (var prereq in cls.MulticlassPrerequisites)
            {
                var score = prereq.RequiredAbility switch
                {
                    Ability.Strength     => character.CharacterStats?.Strength ?? 0,
                    Ability.Dexterity    => character.CharacterStats?.Dexterity ?? 0,
                    Ability.Constitution => character.CharacterStats?.Constitution ?? 0,
                    Ability.Intelligence => character.CharacterStats?.Intelligence ?? 0,
                    Ability.Wisdom       => character.CharacterStats?.Wisdom ?? 0,
                    Ability.Charisma     => character.CharacterStats?.Charisma ?? 0,
                    _                    => 0
                };
                if (score < prereq.MinimumScore)
                    throw new InvalidOperationException(
                        $"Requires {prereq.RequiredAbility} {prereq.MinimumScore}+ to multiclass into {cls.Name}. Current: {score}.");
            }
        }

        var now = DateTime.UtcNow;

        character.Classes.Add(new CharacterClass
        {
            ClassId           = cls.Id,
            ClassLevel        = 1,
            TotalHitDiceCount = 1,
            CreatedAt         = now,
            UpdatedAt         = now
        });

        character.Level += 1;
        character.UpdatedAt = now;

        foreach (var cf in cls.ClassFeatures.Where(cf => cf.GainingLevel == 1 && cf.Name != "Starting Proficiencies"))
        {
            if (!character.Features.Any(f => f.Name == cf.Name))
                character.Features.Add(new Feature { Name = cf.Name, Description = cf.Description, CreatedAt = now, UpdatedAt = now });
        }

        foreach (var t in cls.SpellSlotTemplates.Where(t => t.ClassLevel == 1 && t.TotalSlots > 0))
        {
            if (!character.SpellSlots.Any(s => s.SlotLevel == t.SpellSlotLevel))
                character.SpellSlots.Add(new SpellSlot
                    { SlotLevel = t.SpellSlotLevel, TotalSlots = t.TotalSlots, RemainingSlots = t.TotalSlots, CreatedAt = now, UpdatedAt = now });
        }

        await _repository.SaveChangesAsync(ct);

        var reloaded = await _repository.GetDetailByIdAsync(characterId, ct)!;
        var newEntry = reloaded!.Classes.First(cc => cc.ClassId == cls.Id);
        return CharacterMapper.ToClassDto(newEntry);
    }

    public async Task<LevelUpResponse> LevelUpAsync(
        int characterId, int classId, LevelUpRequest? request, int userId, CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);
        var classEntry = character.Classes.FirstOrDefault(cc => cc.Id == classId)
                         ?? throw new KeyNotFoundException("Class entry not found on this character.");

        var newLevel = classEntry.ClassLevel + 1;

        var cls = await _classRepository.GetByNameWithDetailsAsync(classEntry.Class?.Name ?? string.Empty, ct)
            ?? throw new KeyNotFoundException("Class data not found in database.");

        var featuresAtLevel = cls.ClassFeatures
            .Where(cf => cf.GainingLevel == newLevel && cf.Name != "Starting Proficiencies")
            .ToList();

        var isAsiLevel = featuresAtLevel.Any(f => f.Name == "Ability Score Improvement");

        if (isAsiLevel && (request is null || (!HasScoreChoice(request) && request.NewFeat is null)))
            return new LevelUpResponse
            {
                RequiresAbilityScoreChoice = true,
                Message = "Choose: +2 to one ability, +1/+1 to two abilities, or gain a feat."
            };

        var now = DateTime.UtcNow;

        if (isAsiLevel && request is not null)
        {
            if (request.NewFeat is not null)
            {
                character.Feats.Add(new Feat
                {
                    Name = request.NewFeat.Name, Requirement = request.NewFeat.Requirement,
                    Description = request.NewFeat.Description, CreatedAt = now, UpdatedAt = now
                });
            }
            else
            {
                var total = (request.StrIncrease ?? 0) + (request.DexIncrease ?? 0) + (request.ConIncrease ?? 0)
                          + (request.IntIncrease ?? 0) + (request.WisIncrease ?? 0) + (request.ChaIncrease ?? 0);
                if (total != 2)
                    throw new InvalidOperationException("Ability score increases must total exactly 2.");

                var stats = character.CharacterStats
                            ?? throw new InvalidOperationException("Character has no stats record.");
                if (request.StrIncrease.HasValue) stats.Strength     = Math.Min(20, stats.Strength     + request.StrIncrease.Value);
                if (request.DexIncrease.HasValue) stats.Dexterity    = Math.Min(20, stats.Dexterity    + request.DexIncrease.Value);
                if (request.ConIncrease.HasValue) stats.Constitution = Math.Min(20, stats.Constitution + request.ConIncrease.Value);
                if (request.IntIncrease.HasValue) stats.Intelligence = Math.Min(20, stats.Intelligence + request.IntIncrease.Value);
                if (request.WisIncrease.HasValue) stats.Wisdom       = Math.Min(20, stats.Wisdom       + request.WisIncrease.Value);
                if (request.ChaIncrease.HasValue) stats.Charisma     = Math.Min(20, stats.Charisma     + request.ChaIncrease.Value);
                stats.UpdatedAt = now;
            }
        }

        classEntry.ClassLevel        = newLevel;
        classEntry.TotalHitDiceCount += 1;
        classEntry.UpdatedAt         = now;
        character.Level   += 1;
        character.UpdatedAt = now;

        foreach (var cf in featuresAtLevel)
        {
            if (!character.Features.Any(f => f.Name == cf.Name))
                character.Features.Add(new Feature { Name = cf.Name, Description = cf.Description, CreatedAt = now, UpdatedAt = now });
        }

        foreach (var t in cls.SpellSlotTemplates.Where(t => t.ClassLevel == newLevel && t.TotalSlots > 0))
        {
            var existing = character.SpellSlots.FirstOrDefault(s => s.SlotLevel == t.SpellSlotLevel);
            if (existing is null)
            {
                character.SpellSlots.Add(new SpellSlot
                    { SlotLevel = t.SpellSlotLevel, TotalSlots = t.TotalSlots, RemainingSlots = t.TotalSlots, CreatedAt = now, UpdatedAt = now });
            }
            else if (t.TotalSlots > existing.TotalSlots)
            {
                var diff = t.TotalSlots - existing.TotalSlots;
                existing.TotalSlots    = t.TotalSlots;
                existing.RemainingSlots = Math.Min(existing.RemainingSlots + diff, existing.TotalSlots);
                existing.UpdatedAt     = now;
            }
        }

        await _repository.SaveChangesAsync(ct);

        var reloaded = await _repository.GetDetailByIdAsync(characterId, ct)!;
        return new LevelUpResponse { Character = CharacterMapper.ToDetailDto(reloaded!) };
    }

    public async Task<CharacterClassDto> UpdateCharacterClassAsync(
        int characterId, int classId, UpdateCharacterClassRequest request, int userId, CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);
        var classEntry = character.Classes.FirstOrDefault(cc => cc.Id == classId)
                         ?? throw new KeyNotFoundException("Class entry not found on this character.");

        if (request.UsedHitDice.HasValue)
            character.UsedHitDice = Math.Clamp(request.UsedHitDice.Value, 0, classEntry.TotalHitDiceCount);

        classEntry.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToClassDto(classEntry);
    }

    public async Task DeleteCharacterClassAsync(int characterId, int classId, int userId,
        CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);

        if (character.Classes.Count <= 1)
            throw new InvalidOperationException("Cannot remove the last class from a character.");

        var classEntry = character.Classes.FirstOrDefault(cc => cc.Id == classId)
                         ?? throw new KeyNotFoundException("Class entry not found on this character.");

        character.Level    -= classEntry.ClassLevel;
        character.UpdatedAt = DateTime.UtcNow;
        character.Classes.Remove(classEntry);

        await _repository.SaveChangesAsync(ct);
    }

    // ── Subclass ───────────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<SubclassDto>> GetSubclassesForClassAsync(
        string className, CancellationToken ct = default)
    {
        var subclasses = await _classRepository.GetSubclassesForClassAsync(className, ct);
        return subclasses.Select(CharacterMapper.ToSubclassDto).ToList();
    }

    public async Task<CharacterClassDto> SetSubclassAsync(
        int characterId, int classId, SetSubclassRequest request, int userId, CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);
        var classEntry = character.Classes.FirstOrDefault(cc => cc.Id == classId)
                         ?? throw new KeyNotFoundException("Class entry not found on this character.");

        var subclass = await _classRepository.GetSubclassByIdAsync(request.SubclassId, ct)
            ?? throw new KeyNotFoundException($"Subclass {request.SubclassId} not found.");

        if (subclass.ClassId != classEntry.ClassId)
            throw new InvalidOperationException(
                $"Subclass '{subclass.Name}' does not belong to {classEntry.Class?.Name ?? "this class"}.");

        classEntry.CharacterSubclassId = request.SubclassId;
        classEntry.UpdatedAt           = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);

        var reloaded = await _repository.GetDetailByIdAsync(characterId, ct)!;
        var reloadedEntry = reloaded!.Classes.First(cc => cc.Id == classId);
        return CharacterMapper.ToClassDto(reloadedEntry);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static bool HasScoreChoice(LevelUpRequest req) =>
        req.StrIncrease.HasValue || req.DexIncrease.HasValue || req.ConIncrease.HasValue
        || req.IntIncrease.HasValue || req.WisIncrease.HasValue || req.ChaIncrease.HasValue;

    private static void ApplyRaceProficiencies(CharacterProficiencies profs, Race race, DateTime now)
    {
        foreach (var wp in race.WeaponGrants)
            profs.Weapons.Add(new WeaponProficiency { Name = wp.Name, CreatedAt = now, UpdatedAt = now });
        foreach (var ap in race.ArmorGrants)
            profs.Armors.Add(new ArmorProficiency { Name = ap.Name, CreatedAt = now, UpdatedAt = now });
        foreach (var lg in race.LanguageGrants)
            profs.Languages.Add(new Language { Name = lg.Name, CreatedAt = now, UpdatedAt = now });
        foreach (var ig in race.InstrumentGrants)
            profs.Instruments.Add(new Instrument { Name = ig.Name, CreatedAt = now, UpdatedAt = now });
    }

    private static void ApplyClassLevel1Proficiencies(CharacterProficiencies profs, Class cls, DateTime now)
    {
        var startingFeature = cls.ClassFeatures.FirstOrDefault(cf => cf.GainingLevel == 1 && cf.Name == "Starting Proficiencies");
        if (startingFeature is null) return;

        foreach (var wp in startingFeature.WeaponGrants)
            profs.Weapons.Add(new WeaponProficiency { Name = wp.Name, CreatedAt = now, UpdatedAt = now });
        foreach (var ap in startingFeature.ArmorGrants)
            profs.Armors.Add(new ArmorProficiency { Name = ap.Name, CreatedAt = now, UpdatedAt = now });
        foreach (var lg in startingFeature.LanguageGrants)
            profs.Languages.Add(new Language { Name = lg.Name, CreatedAt = now, UpdatedAt = now });
        foreach (var ig in startingFeature.InstrumentGrants)
            profs.Instruments.Add(new Instrument { Name = ig.Name, CreatedAt = now, UpdatedAt = now });
    }

    private static void ApplyPlayerChoices(CharacterProficiencies profs, CreateCharacterRequest request, DateTime now)
    {
        foreach (var skillName in request.SkillChoices)
        {
            if (Enum.TryParse<Skill>(skillName, true, out var skill))
                profs.Skills.Add(new SkillProficiency { Skill = skill, HasExpertise = false, CreatedAt = now, UpdatedAt = now });
        }
        foreach (var w in request.WeaponChoices)
            profs.Weapons.Add(new WeaponProficiency { Name = w, CreatedAt = now, UpdatedAt = now });
        foreach (var a in request.ArmorChoices)
            profs.Armors.Add(new ArmorProficiency { Name = a, CreatedAt = now, UpdatedAt = now });
        foreach (var l in request.LanguageChoices)
            profs.Languages.Add(new Language { Name = l, CreatedAt = now, UpdatedAt = now });
        foreach (var i in request.InstrumentChoices)
            profs.Instruments.Add(new Instrument { Name = i, CreatedAt = now, UpdatedAt = now });
    }

    private async Task<Character> GetOwnedDetailAsync(int id, int userId, CancellationToken ct)
    {
        var c = await _repository.GetDetailByIdAsync(id, ct)
                ?? throw new KeyNotFoundException("Character not found.");
        if (c.OwnerId != userId) throw new UnauthorizedAccessException("Access denied.");
        return c;
    }
}
