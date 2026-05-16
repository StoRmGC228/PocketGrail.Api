namespace PocketGrail.Application.Services;

using PocketGrail.Application.Data;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Mappers;
using PocketGrail.Domain.Entities;

public sealed class CharacterService : ICharacterService
{
    private readonly ICharacterRepository _repository;
    private readonly ICloudinaryService _cloudinaryService;

    public CharacterService(ICharacterRepository repository, ICloudinaryService cloudinaryService)
    {
        _repository = repository;
        _cloudinaryService = cloudinaryService;
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
        string? imageUrl = null;
        if (request.Image is not null)
            imageUrl = await _cloudinaryService.UploadImageAsync(request.Image, ct: ct);

        var now = DateTime.UtcNow;
        var character = new Character
        {
            Name = request.Name,
            Race = request.Race,
            Level = 1,
            CurrentHp = 0,
            MaxHp = 0,
            ImageUrl = imageUrl,
            OwnerId = userId,
            CampaignId = request.CampaignId,
            SpellAbility = DnD5eData.GetSpellAbility(request.ClassName),
            CreatedAt = now,
            UpdatedAt = now
        };

        var wallet = new CharacterWallet
        {
            CharacterId = 0,
            CreatedAt = now,
            UpdatedAt = now
        };
        character.Wallet = wallet;

        await _repository.AddAsync(character, ct);
        await _repository.SaveChangesAsync(ct);

        await AddClassToCharacterInternalAsync(character.Id, request.ClassName, isFirstClass: true, ct);

        var created = await _repository.GetByIdAsync(character.Id, ct)
            ?? throw new InvalidOperationException("Failed to retrieve created character.");
        return CharacterMapper.ToDto(created);
    }

    public async Task<CharacterDto> UpdateCharacterAsync(
        int id, UpdateCharacterRequest request, int userId, CancellationToken ct = default)
    {
        var character = await _repository.GetDetailByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Character not found.");
        if (character.OwnerId != userId) throw new UnauthorizedAccessException("Only the character owner can update it.");

        if (request.Name is not null) character.Name = request.Name;
        if (request.Race is not null) character.Race = request.Race;
        if (request.CurrentHp.HasValue) character.CurrentHp = request.CurrentHp.Value;
        if (request.MaxHp.HasValue) character.MaxHp = request.MaxHp.Value;
        if (request.CampaignId.HasValue) character.CampaignId = request.CampaignId.Value;
        if (request.Alignment is not null) character.Alignment = request.Alignment;
        if (request.SpellAbility is not null) character.SpellAbility = request.SpellAbility;
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
        if (character.OwnerId != userId) throw new UnauthorizedAccessException("Only the character owner can delete it.");
        await _repository.DeleteAsync(character, ct);
        await _repository.SaveChangesAsync(ct);
    }

    // ── Stats / Vitals / Wallet / Image ───────────────────────────────────────

    public async Task<CharacterDetailDto> UpdateStatsAsync(int id, UpdateStatsRequest request, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(id, userId, ct);
        if (request.StrScore.HasValue) c.StrScore = request.StrScore.Value;
        if (request.DexScore.HasValue) c.DexScore = request.DexScore.Value;
        if (request.ConScore.HasValue) c.ConScore = request.ConScore.Value;
        if (request.IntScore.HasValue) c.IntScore = request.IntScore.Value;
        if (request.WisScore.HasValue) c.WisScore = request.WisScore.Value;
        if (request.ChaScore.HasValue) c.ChaScore = request.ChaScore.Value;
        if (request.ArmorClass.HasValue) c.ArmorClass = request.ArmorClass.Value;
        if (request.Speed.HasValue) c.Speed = request.Speed.Value;
        if (request.SpellAbility is not null) c.SpellAbility = request.SpellAbility;
        if (request.Alignment is not null) c.Alignment = request.Alignment;
        c.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToDetailDto(c);
    }

    public async Task<CharacterDetailDto> UpdateVitalsAsync(int id, UpdateVitalsRequest request, int userId, CancellationToken ct = default)
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

    public async Task<CharacterDetailDto> UpdateWalletAsync(int id, UpdateWalletRequest request, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(id, userId, ct);
        var wallet = c.Wallet ?? new CharacterWallet { CharacterId = c.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
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

    public async Task<CharacterDetailDto> UpdateImageAsync(int id, UpdateCharacterImageRequest request, int userId, CancellationToken ct = default)
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

    public async Task<ItemDto> AddItemAsync(int characterId, AddItemRequest request, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var item = new Item
        {
            Name = request.Name,
            Description = request.Description,
            Rarity = request.Rarity,
            Category = request.Category,
            Weight = request.Weight,
            Cost = request.Cost,
            IsWeapon = request.IsWeapon,
            IsMagical = request.IsMagical,
            AtkMod = request.AtkMod,
            Damage = request.Damage,
            DamageType = request.DamageType,
            WeaponProperties = request.WeaponProperties,
            ChargesInfo = request.ChargesInfo,
            RechargeType = request.RechargeType,
            Tags = request.Tags,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
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

    public async Task<ItemDto> UpdateItemAsync(int characterId, int itemId, UpdateItemRequest request, int userId, CancellationToken ct = default)
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

    public async Task<SpellDto> AddSpellAsync(int characterId, AddSpellRequest request, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var spell = new Spell
        {
            Name = request.Name,
            Level = request.Level,
            School = request.School,
            Range = request.Range,
            CastingTime = request.CastingTime,
            Concentration = request.Concentration,
            IsRitual = request.IsRitual,
            Components = request.Components,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
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

    public async Task<SpellDto> ToggleSpellPreparedAsync(int characterId, int spellId, int userId, CancellationToken ct = default)
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

    public async Task<SpellSlotDto> UpdateSpellSlotAsync(int characterId, UpdateSpellSlotRequest request, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var slot = c.SpellSlots.FirstOrDefault(s => s.SlotLevel == request.SlotLevel)
            ?? throw new KeyNotFoundException($"No spell slot of level {request.SlotLevel} found.");
        slot.RemainingSlots = Math.Clamp(request.RemainingSlots, 0, slot.TotalSlots);
        await _repository.SaveChangesAsync(ct);
        return new SpellSlotDto { Id = slot.Id, SlotLevel = slot.SlotLevel, TotalSlots = slot.TotalSlots, RemainingSlots = slot.RemainingSlots };
    }

    // ── Feats ──────────────────────────────────────────────────────────────────

    public async Task<FeatDto> AddFeatAsync(int characterId, AddFeatRequest request, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var feat = new Feat
        {
            Name = request.Name,
            Requirement = request.Requirement,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
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

    public async Task<FeatureDto> AddFeatureAsync(int characterId, AddFeatureRequest request, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var feature = new Feature
        {
            Name = request.Name,
            Description = request.Description,
            FeatureType = request.FeatureType,
            FeatureLevel = request.FeatureLevel,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        c.Features.Add(feature);
        await _repository.SaveChangesAsync(ct);

        var junction = feature.CharacterFeatures.FirstOrDefault(cf => cf.CharacterId == characterId);
        return CharacterMapper.ToFeatureDto(feature, junction?.IsAutoAdded ?? false);
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

    public async Task<ProficiencyDto> AddProficiencyAsync(int characterId, AddProficiencyRequest request, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var proficiency = new Proficiency
        {
            Name = request.Name,
            ProficiencyType = request.ProficiencyType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        c.Proficiencies.Add(proficiency);
        await _repository.SaveChangesAsync(ct);

        var junction = proficiency.CharacterProficiencies.FirstOrDefault(cp => cp.CharacterId == characterId);
        if (junction is not null)
        {
            junction.HasExpertise = request.HasExpertise;
            junction.AbilityKey = request.AbilityKey;
            await _repository.SaveChangesAsync(ct);
        }

        return new ProficiencyDto
        {
            Id = proficiency.Id,
            Name = proficiency.Name,
            ProficiencyType = proficiency.ProficiencyType,
            HasExpertise = request.HasExpertise,
            AbilityKey = request.AbilityKey
        };
    }

    public async Task DeleteProficiencyAsync(int characterId, int proficiencyId, int userId, CancellationToken ct = default)
    {
        var c = await GetOwnedDetailAsync(characterId, userId, ct);
        var proficiency = c.Proficiencies.FirstOrDefault(p => p.Id == proficiencyId)
            ?? throw new KeyNotFoundException("Proficiency not found on this character.");
        c.Proficiencies.Remove(proficiency);
        await _repository.SaveChangesAsync(ct);
    }

    // ── Allies ─────────────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<AllyDto>> GetAlliesAsync(int characterId, int userId, CancellationToken ct = default)
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
                CharacterId = c.Id,
                CharacterName = c.Name,
                Race = c.Race,
                Classes = c.Classes.Select(CharacterMapper.ToClassDto).ToList(),
                ClassDisplay = CharacterMapper.FormatClassDisplay(c.Classes),
                Level = c.Level,
                CurrentHp = c.CurrentHp,
                MaxHp = c.MaxHp,
                ImageUrl = c.ImageUrl,
                ImageCropX = c.ImageCropX,
                ImageCropY = c.ImageCropY,
                ImageCropWidth = c.ImageCropWidth,
                ImageCropHeight = c.ImageCropHeight,
                UserId = c.OwnerId,
                Username = c.Owner?.Username ?? string.Empty
            })
            .ToList();
    }

    // ── Character class management ─────────────────────────────────────────────

    public async Task<CharacterClassDto> AddCharacterClassAsync(
        int characterId, AddCharacterClassRequest request, int userId, CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);

        if (character.Classes.Any(cc => cc.ClassName.Equals(request.ClassName, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Character already has a level in {request.ClassName}.");

        await AddClassToCharacterInternalAsync(characterId, request.ClassName, isFirstClass: false, ct);

        var reloaded = await _repository.GetDetailByIdAsync(characterId, ct)!;
        var newClass = reloaded!.Classes.First(cc => cc.ClassName.Equals(request.ClassName, StringComparison.OrdinalIgnoreCase));
        return CharacterMapper.ToClassDto(newClass);
    }

    public async Task<CharacterClassDto> LevelUpAsync(
        int characterId, int classId, int userId, CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);
        var classEntry = character.Classes.FirstOrDefault(cc => cc.Id == classId)
            ?? throw new KeyNotFoundException("Class entry not found on this character.");

        classEntry.ClassLevel += 1;
        classEntry.TotalHitDice += 1;
        character.Level += 1;
        character.UpdatedAt = DateTime.UtcNow;
        classEntry.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync(ct);
        await SeedClassFeaturesAtLevel(characterId, classEntry.ClassName, classEntry.ClassLevel, ct);

        return CharacterMapper.ToClassDto(classEntry);
    }

    public async Task<CharacterClassDto> UpdateCharacterClassAsync(
        int characterId, int classId, UpdateCharacterClassRequest request, int userId, CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);
        var classEntry = character.Classes.FirstOrDefault(cc => cc.Id == classId)
            ?? throw new KeyNotFoundException("Class entry not found on this character.");

        if (request.Subclass is not null) classEntry.Subclass = request.Subclass;
        if (request.UsedHitDice.HasValue)
            classEntry.UsedHitDice = Math.Clamp(request.UsedHitDice.Value, 0, classEntry.TotalHitDice);

        classEntry.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);
        return CharacterMapper.ToClassDto(classEntry);
    }

    public async Task DeleteCharacterClassAsync(int characterId, int classId, int userId, CancellationToken ct = default)
    {
        var character = await GetOwnedDetailAsync(characterId, userId, ct);

        if (character.Classes.Count <= 1)
            throw new InvalidOperationException("Cannot remove the last class from a character.");

        var classEntry = character.Classes.FirstOrDefault(cc => cc.Id == classId)
            ?? throw new KeyNotFoundException("Class entry not found on this character.");

        var className = classEntry.ClassName;
        var classLevel = classEntry.ClassLevel;

        character.Classes.Remove(classEntry);
        character.Level -= classLevel;
        character.UpdatedAt = DateTime.UtcNow;

        // Remove auto-added class features from this class
        var featuresToRemove = character.Features
            .Where(f => string.Equals(f.SourceClass, className, StringComparison.OrdinalIgnoreCase)
                     && f.CharacterFeatures.Any(cf => cf.CharacterId == characterId && cf.IsAutoAdded))
            .ToList();
        foreach (var f in featuresToRemove)
            character.Features.Remove(f);

        await _repository.SaveChangesAsync(ct);
    }

    // ── Seeding helpers ────────────────────────────────────────────────────────

    private async Task AddClassToCharacterInternalAsync(int characterId, string className, bool isFirstClass, CancellationToken ct)
    {
        var character = await _repository.GetDetailByIdAsync(characterId, ct)
            ?? throw new InvalidOperationException("Character not found.");

        var now = DateTime.UtcNow;
        var hitDice = DnD5eData.GetHitDice(className);

        var characterClass = new CharacterClass
        {
            CharacterId = characterId,
            ClassName = className,
            ClassLevel = 1,
            HitDice = hitDice,
            TotalHitDice = 1,
            UsedHitDice = 0,
            CreatedAt = now,
            UpdatedAt = now
        };
        character.Classes.Add(characterClass);

        if (isFirstClass)
        {
            // Seed saving throw proficiencies (first class only per D&D 5e multiclass rules)
            foreach (var abilityKey in DnD5eData.GetSavingThrows(className))
            {
                var proficiency = new Proficiency
                {
                    Name = $"{abilityKey.ToUpperInvariant()} saving throw",
                    ProficiencyType = "saving_throw",
                    CreatedAt = now,
                    UpdatedAt = now
                };
                character.Proficiencies.Add(proficiency);
            }

            await _repository.SaveChangesAsync(ct);

            // Set AbilityKey on the saving throw proficiency junctions
            var reloaded = await _repository.GetDetailByIdAsync(characterId, ct);
            if (reloaded is not null)
            {
                var savingThrows = DnD5eData.GetSavingThrows(className);
                foreach (var p in reloaded.Proficiencies.Where(p => p.ProficiencyType == "saving_throw"))
                {
                    var junction = p.CharacterProficiencies.FirstOrDefault(cp => cp.CharacterId == characterId);
                    if (junction is null) continue;
                    var key = savingThrows.FirstOrDefault(k => p.Name.StartsWith(k, StringComparison.OrdinalIgnoreCase));
                    if (key is not null) junction.AbilityKey = key;
                }
                await _repository.SaveChangesAsync(ct);
            }

            await SeedCharacterDefaults(characterId, character.Race, className, 1, ct);
        }
        else
        {
            // Multiclass: only seed level-1 class features (no saving throws)
            await _repository.SaveChangesAsync(ct);
            await SeedClassFeaturesAtLevel(characterId, className, 1, ct);

            // Increment character total level
            var c = await _repository.GetByIdAsync(characterId, ct);
            if (c is not null)
            {
                c.Level += 1;
                c.UpdatedAt = DateTime.UtcNow;
                await _repository.SaveChangesAsync(ct);
            }
        }
    }

    private async Task SeedCharacterDefaults(int characterId, string race, string className, int level, CancellationToken ct)
    {
        var c = await _repository.GetDetailByIdAsync(characterId, ct)
            ?? throw new InvalidOperationException("Character not found after creation.");

        var now = DateTime.UtcNow;

        foreach (var rf in DnD5eData.GetRaceFeatures(race))
        {
            var feature = new Feature { Name = rf.Name, Description = rf.Description, FeatureType = "race", SourceRace = race, CreatedAt = now, UpdatedAt = now };
            c.Features.Add(feature);
        }

        foreach (var cf in DnD5eData.GetClassFeaturesUpToLevel(className, level))
        {
            var feature = new Feature { Name = cf.Name, Description = cf.Description, FeatureType = "class", FeatureLevel = cf.Level, SourceClass = className, CreatedAt = now, UpdatedAt = now };
            c.Features.Add(feature);
        }

        foreach (var pf in DnD5eData.GetClassProficiencies(className))
        {
            var proficiency = new Proficiency { Name = pf.Name, ProficiencyType = pf.Type, CreatedAt = now, UpdatedAt = now };
            c.Proficiencies.Add(proficiency);
        }

        var slotRows = DnD5eData.GetSpellSlots(className, level);
        foreach (var row in slotRows)
            c.SpellSlots.Add(new SpellSlot { SlotLevel = row[0], TotalSlots = row[1], RemainingSlots = row[1], CreatedAt = now, UpdatedAt = now });

        await _repository.SaveChangesAsync(ct);

        var reloaded = await _repository.GetDetailByIdAsync(characterId, ct);
        if (reloaded is null) return;
        foreach (var feature in reloaded.Features)
            foreach (var cf in feature.CharacterFeatures.Where(cf => cf.CharacterId == characterId))
                cf.IsAutoAdded = true;
        await _repository.SaveChangesAsync(ct);
    }

    private async Task SeedClassFeaturesAtLevel(int characterId, string className, int level, CancellationToken ct)
    {
        var c = await _repository.GetDetailByIdAsync(characterId, ct);
        if (c is null) return;
        var now = DateTime.UtcNow;
        foreach (var cf in DnD5eData.GetClassFeaturesAtLevel(className, level))
        {
            if (c.Features.Any(f => f.Name == cf.Name && f.SourceClass == className)) continue;
            var feature = new Feature { Name = cf.Name, Description = cf.Description, FeatureType = "class", FeatureLevel = cf.Level, SourceClass = className, CreatedAt = now, UpdatedAt = now };
            c.Features.Add(feature);
        }
        await _repository.SaveChangesAsync(ct);

        var reloaded = await _repository.GetDetailByIdAsync(characterId, ct);
        if (reloaded is null) return;
        foreach (var feature in reloaded.Features.Where(f => f.FeatureLevel == level && f.SourceClass == className))
            foreach (var cf in feature.CharacterFeatures.Where(cf => cf.CharacterId == characterId && !cf.IsAutoAdded))
                cf.IsAutoAdded = true;
        await _repository.SaveChangesAsync(ct);
    }

    private async Task<Character> GetOwnedDetailAsync(int id, int userId, CancellationToken ct)
    {
        var c = await _repository.GetDetailByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Character not found.");
        if (c.OwnerId != userId) throw new UnauthorizedAccessException("Access denied.");
        return c;
    }
}
