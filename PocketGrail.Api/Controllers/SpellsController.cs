namespace PocketGrail.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Mappers;
using PocketGrail.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SpellsController : ControllerBase
{
    private readonly ISpellRepository _spellRepository;

    public SpellsController(ISpellRepository spellRepository)
    {
        _spellRepository = spellRepository;
    }

    // GET /api/spells
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var spells = await _spellRepository.GetAllAsync(ct);
        return Ok(spells.Select(CharacterMapper.ToCatalogSpellDto));
    }

    // GET /api/spells/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var spell = await _spellRepository.GetByIdAsync(id, ct);
        return spell is null ? NotFound() : Ok(CharacterMapper.ToCatalogSpellDto(spell));
    }

    // POST /api/spells
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCatalogSpellRequest request, CancellationToken ct)
    {
        var spell = new Spell
        {
            Name        = request.Name,
            Level       = request.Level,
            School      = request.School,
            Range       = request.Range,
            CastingTime = request.CastingTime,
            Concentration = request.Concentration,
            IsRitual    = request.IsRitual,
            Components  = request.Components,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow,
        };
        await _spellRepository.AddAsync(spell, ct);
        await _spellRepository.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = spell.Id }, CharacterMapper.ToCatalogSpellDto(spell));
    }

    // PUT /api/spells/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCatalogSpellRequest request, CancellationToken ct)
    {
        var spell = await _spellRepository.GetByIdAsync(id, ct);
        if (spell is null) return NotFound();

        if (request.Name is not null)            spell.Name          = request.Name;
        if (request.Level.HasValue)              spell.Level         = request.Level.Value;
        if (request.School is not null)          spell.School        = request.School;
        if (request.Range is not null)           spell.Range         = request.Range;
        if (request.CastingTime is not null)     spell.CastingTime   = request.CastingTime;
        if (request.Concentration.HasValue)      spell.Concentration = request.Concentration.Value;
        if (request.IsRitual.HasValue)           spell.IsRitual      = request.IsRitual.Value;
        if (request.Components is not null)      spell.Components    = request.Components;
        spell.UpdatedAt = DateTime.UtcNow;

        await _spellRepository.SaveChangesAsync(ct);
        return Ok(CharacterMapper.ToCatalogSpellDto(spell));
    }

    // DELETE /api/spells/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var spell = await _spellRepository.GetByIdAsync(id, ct);
        if (spell is null) return NotFound();

        await _spellRepository.DeleteAsync(spell, ct);
        await _spellRepository.SaveChangesAsync(ct);
        return NoContent();
    }
}
