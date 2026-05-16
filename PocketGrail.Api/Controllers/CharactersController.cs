namespace PocketGrail.Api.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CharactersController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharactersController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    // GET /api/characters/mine
    [HttpGet("mine")]
    public async Task<IActionResult> GetMyCharacters(CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var characters = await _characterService.GetMyCharactersAsync(userId, ct);
        return Ok(characters);
    }

    // GET /api/characters/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var character = await _characterService.GetByIdAsync(id, ct);
        return character is null ? NotFound() : Ok(character);
    }

    // POST /api/characters  — multipart/form-data
    [HttpPost]
    public async Task<IActionResult> CreateCharacter(
        [FromForm] CreateCharacterRequest request,
        CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var character = await _characterService.CreateCharacterAsync(request, userId, ct);
        return CreatedAtAction(nameof(GetById), new { id = character.Id }, character);
    }

    // PUT /api/characters/{id}  — multipart/form-data
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCharacter(
        int id,
        [FromForm] UpdateCharacterRequest request,
        CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var character = await _characterService.UpdateCharacterAsync(id, request, userId, ct);
        return Ok(character);
    }

    // DELETE /api/characters/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCharacter(int id, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        await _characterService.DeleteCharacterAsync(id, userId, ct);
        return NoContent();
    }

    // GET /api/characters/{id}/sheet
    [HttpGet("{id:int}/sheet")]
    public async Task<IActionResult> GetCharacterDetail(int id, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var detail = await _characterService.GetCharacterDetailAsync(id, userId, ct);
        return detail is null ? NotFound() : Ok(detail);
    }

    // PUT /api/characters/{id}/stats
    [HttpPut("{id:int}/stats")]
    public async Task<IActionResult> UpdateStats(int id, [FromBody] UpdateStatsRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var detail = await _characterService.UpdateStatsAsync(id, request, userId, ct);
        return Ok(detail);
    }

    // PUT /api/characters/{id}/vitals
    [HttpPut("{id:int}/vitals")]
    public async Task<IActionResult> UpdateVitals(int id, [FromBody] UpdateVitalsRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var detail = await _characterService.UpdateVitalsAsync(id, request, userId, ct);
        return Ok(detail);
    }

    // PUT /api/characters/{id}/wallet
    [HttpPut("{id:int}/wallet")]
    public async Task<IActionResult> UpdateWallet(int id, [FromBody] UpdateWalletRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var detail = await _characterService.UpdateWalletAsync(id, request, userId, ct);
        return Ok(detail);
    }

    // PUT /api/characters/{id}/image  — multipart/form-data
    [HttpPut("{id:int}/image")]
    public async Task<IActionResult> UpdateImage(int id, [FromForm] UpdateCharacterImageRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var detail = await _characterService.UpdateImageAsync(id, request, userId, ct);
        return Ok(detail);
    }

    // POST /api/characters/{id}/items
    [HttpPost("{id:int}/items")]
    public async Task<IActionResult> AddItem(int id, [FromBody] AddItemRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var item = await _characterService.AddItemAsync(id, request, userId, ct);
        return Ok(item);
    }

    // PUT /api/characters/{id}/items/{itemId}
    [HttpPut("{id:int}/items/{itemId:int}")]
    public async Task<IActionResult> UpdateItem(int id, int itemId, [FromBody] UpdateItemRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var item = await _characterService.UpdateItemAsync(id, itemId, request, userId, ct);
        return Ok(item);
    }

    // DELETE /api/characters/{id}/items/{itemId}
    [HttpDelete("{id:int}/items/{itemId:int}")]
    public async Task<IActionResult> DeleteItem(int id, int itemId, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        await _characterService.DeleteItemAsync(id, itemId, userId, ct);
        return NoContent();
    }

    // POST /api/characters/{id}/spells
    [HttpPost("{id:int}/spells")]
    public async Task<IActionResult> AddSpell(int id, [FromBody] AddSpellRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var spell = await _characterService.AddSpellAsync(id, request, userId, ct);
        return Ok(spell);
    }

    // PATCH /api/characters/{id}/spells/{spellId}/toggle-prepared
    [HttpPatch("{id:int}/spells/{spellId:int}/toggle-prepared")]
    public async Task<IActionResult> ToggleSpellPrepared(int id, int spellId, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var detail = await _characterService.ToggleSpellPreparedAsync(id, spellId, userId, ct);
        return Ok(detail);
    }

    // DELETE /api/characters/{id}/spells/{spellId}
    [HttpDelete("{id:int}/spells/{spellId:int}")]
    public async Task<IActionResult> DeleteSpell(int id, int spellId, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        await _characterService.DeleteSpellAsync(id, spellId, userId, ct);
        return NoContent();
    }

    // PUT /api/characters/{id}/spell-slots
    [HttpPut("{id:int}/spell-slots")]
    public async Task<IActionResult> UpdateSpellSlot(int id, [FromBody] UpdateSpellSlotRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var slot = await _characterService.UpdateSpellSlotAsync(id, request, userId, ct);
        return Ok(slot);
    }

    // POST /api/characters/{id}/feats
    [HttpPost("{id:int}/feats")]
    public async Task<IActionResult> AddFeat(int id, [FromBody] AddFeatRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var feat = await _characterService.AddFeatAsync(id, request, userId, ct);
        return Ok(feat);
    }

    // DELETE /api/characters/{id}/feats/{featId}
    [HttpDelete("{id:int}/feats/{featId:int}")]
    public async Task<IActionResult> DeleteFeat(int id, int featId, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        await _characterService.DeleteFeatAsync(id, featId, userId, ct);
        return NoContent();
    }

    // POST /api/characters/{id}/features
    [HttpPost("{id:int}/features")]
    public async Task<IActionResult> AddFeature(int id, [FromBody] AddFeatureRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var feature = await _characterService.AddFeatureAsync(id, request, userId, ct);
        return Ok(feature);
    }

    // DELETE /api/characters/{id}/features/{featureId}
    [HttpDelete("{id:int}/features/{featureId:int}")]
    public async Task<IActionResult> DeleteFeature(int id, int featureId, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        await _characterService.DeleteFeatureAsync(id, featureId, userId, ct);
        return NoContent();
    }

    // POST /api/characters/{id}/proficiencies
    [HttpPost("{id:int}/proficiencies")]
    public async Task<IActionResult> AddProficiency(int id, [FromBody] AddProficiencyRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var proficiency = await _characterService.AddProficiencyAsync(id, request, userId, ct);
        return Ok(proficiency);
    }

    // DELETE /api/characters/{id}/proficiencies/{proficiencyId}
    [HttpDelete("{id:int}/proficiencies/{proficiencyId:int}")]
    public async Task<IActionResult> DeleteProficiency(int id, int proficiencyId, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        await _characterService.DeleteProficiencyAsync(id, proficiencyId, userId, ct);
        return NoContent();
    }

    // GET /api/characters/{id}/allies
    [HttpGet("{id:int}/allies")]
    public async Task<IActionResult> GetAllies(int id, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var allies = await _characterService.GetAlliesAsync(id, userId, ct);
        return Ok(allies);
    }

    // POST /api/characters/{id}/classes  — multiclass: add a new class at level 1
    [HttpPost("{id:int}/classes")]
    public async Task<IActionResult> AddCharacterClass(int id, [FromBody] AddCharacterClassRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var result = await _characterService.AddCharacterClassAsync(id, request, userId, ct);
        return Ok(result);
    }

    // POST /api/characters/{id}/classes/{classId}/level-up
    [HttpPost("{id:int}/classes/{classId:int}/level-up")]
    public async Task<IActionResult> LevelUp(int id, int classId, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var result = await _characterService.LevelUpAsync(id, classId, userId, ct);
        return Ok(result);
    }

    // PATCH /api/characters/{id}/classes/{classId}
    [HttpPatch("{id:int}/classes/{classId:int}")]
    public async Task<IActionResult> UpdateCharacterClass(int id, int classId, [FromBody] UpdateCharacterClassRequest request, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        var result = await _characterService.UpdateCharacterClassAsync(id, classId, request, userId, ct);
        return Ok(result);
    }

    // DELETE /api/characters/{id}/classes/{classId}
    [HttpDelete("{id:int}/classes/{classId:int}")]
    public async Task<IActionResult> DeleteCharacterClass(int id, int classId, CancellationToken ct)
    {
        var userId = GetUserIdFromClaims();
        await _characterService.DeleteCharacterClassAsync(id, classId, userId, ct);
        return NoContent();
    }

    private int GetUserIdFromClaims()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User id claim missing.");
        return int.Parse(raw);
    }
}
