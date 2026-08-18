namespace PocketGrail.Application.Interfaces;

using PocketGrail.Application.DTOs;

public interface ICharacterService
{
    Task<IReadOnlyList<CharacterDto>> GetMyCharactersAsync(int userId, CancellationToken ct = default);
    Task<CharacterDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CharacterDetailDto?> GetCharacterDetailAsync(int id, int userId, CancellationToken ct = default);
    Task<CharacterDto> CreateCharacterAsync(CreateCharacterRequest request, int userId, CancellationToken ct = default);
    Task<CharacterDto> UpdateCharacterAsync(int id, UpdateCharacterRequest request, int userId, CancellationToken ct = default);
    Task DeleteCharacterAsync(int id, int userId, CancellationToken ct = default);

    Task<CharacterDetailDto> UpdateStatsAsync(int id, UpdateStatsRequest request, int userId, CancellationToken ct = default);
    Task<CharacterDetailDto> UpdateVitalsAsync(int id, UpdateVitalsRequest request, int userId, CancellationToken ct = default);
    Task<CharacterDetailDto> UpdateWalletAsync(int id, UpdateWalletRequest request, int userId, CancellationToken ct = default);
    Task<CharacterDetailDto> UpdateImageAsync(int id, UpdateCharacterImageRequest request, int userId, CancellationToken ct = default);

    Task<ItemDto> AddItemAsync(int characterId, AddItemRequest request, int userId, CancellationToken ct = default);
    Task<ItemDto> AddItemFromCatalogAsync(int characterId, int itemId, int userId, CancellationToken ct = default);
    Task<ItemDto> UpdateItemAsync(int characterId, int itemId, UpdateItemRequest request, int userId, CancellationToken ct = default);
    Task DeleteItemAsync(int characterId, int itemId, int userId, CancellationToken ct = default);

    Task<SpellDto> AddSpellAsync(int characterId, AddSpellRequest request, int userId, CancellationToken ct = default);
    Task<SpellDto> AddSpellFromCatalogAsync(int characterId, int spellId, int userId, CancellationToken ct = default);
    Task<SpellDto> ToggleSpellPreparedAsync(int characterId, int spellId, int userId, CancellationToken ct = default);
    Task DeleteSpellAsync(int characterId, int spellId, int userId, CancellationToken ct = default);
    Task<SpellSlotDto> UpdateSpellSlotAsync(int characterId, UpdateSpellSlotRequest request, int userId, CancellationToken ct = default);

    Task<FeatDto> AddFeatAsync(int characterId, AddFeatRequest request, int userId, CancellationToken ct = default);
    Task DeleteFeatAsync(int characterId, int featId, int userId, CancellationToken ct = default);

    Task<FeatureDto> AddFeatureAsync(int characterId, AddFeatureRequest request, int userId, CancellationToken ct = default);
    Task DeleteFeatureAsync(int characterId, int featureId, int userId, CancellationToken ct = default);

    Task<ProficiencyDto> AddProficiencyAsync(int characterId, AddProficiencyRequest request, int userId, CancellationToken ct = default);
    Task DeleteProficiencyAsync(int characterId, int proficiencyId, int userId, CancellationToken ct = default);

    Task<IReadOnlyList<AllyDto>> GetAlliesAsync(int characterId, int userId, CancellationToken ct = default);

    Task<CharacterClassDto> AddCharacterClassAsync(int characterId, AddCharacterClassRequest request, int userId, CancellationToken ct = default);
    Task<LevelUpResponse> LevelUpAsync(int characterId, int classId, LevelUpRequest? request, int userId, CancellationToken ct = default);
    Task<CharacterClassDto> UpdateCharacterClassAsync(int characterId, int classId, UpdateCharacterClassRequest request, int userId, CancellationToken ct = default);
    Task DeleteCharacterClassAsync(int characterId, int classId, int userId, CancellationToken ct = default);

    Task<IReadOnlyList<SubclassDto>> GetSubclassesForClassAsync(string className, CancellationToken ct = default);
    Task<CharacterClassDto> SetSubclassAsync(int characterId, int classId, SetSubclassRequest request, int userId, CancellationToken ct = default);
}
