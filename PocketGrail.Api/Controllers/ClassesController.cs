namespace PocketGrail.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Mappers;
using PocketGrail.Domain.Entities.ClassEntities;
using PocketGrail.Domain.Entities.Enums;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ClassesController : ControllerBase
{
    private readonly IClassRepository _classRepository;

    public ClassesController(IClassRepository classRepository)
    {
        _classRepository = classRepository;
    }

    // GET /api/classes
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var classes = await _classRepository.GetAllAsync(ct);
        return Ok(classes.Select(CharacterMapper.ToClassInfoDto).ToList());
    }

    // GET /api/classes/{className}/subclasses
    [HttpGet("{className}/subclasses")]
    public async Task<IActionResult> GetSubclasses(string className, CancellationToken ct)
    {
        var subclasses = await _classRepository.GetSubclassesForClassAsync(className, ct);
        return Ok(subclasses.Select(CharacterMapper.ToSubclassDto).ToList());
    }

    // GET /api/classes/{className}/starting-items
    [HttpGet("{className}/starting-items")]
    public async Task<IActionResult> GetStartingItems(string className, CancellationToken ct)
    {
        var set = await _classRepository.GetStartingItemsForClassAsync(className, ct);
        if (set is null)
            return Ok(new ClassStartingItemSetDto { ChoicePairs = [] });
        return Ok(CharacterMapper.ToStartingItemSetDto(set));
    }

    // GET /api/classes/{className}/saving-throws
    [HttpGet("{className}/saving-throws")]
    public async Task<IActionResult> GetSavingThrows(string className, CancellationToken ct)
    {
        var savingThrows = await _classRepository.GetSavingThrowsAsync(className, ct);
        return Ok(savingThrows.Select(CharacterMapper.ToClassSavingThrowDto).ToList());
    }

    // POST /api/classes/{className}/saving-throws
    [HttpPost("{className}/saving-throws")]
    public async Task<IActionResult> AddSavingThrow(string className, [FromBody] AddClassSavingThrowRequest request,
        CancellationToken ct)
    {
        var @class = await _classRepository.GetByNameAsync(className, ct);
        if (@class is null)
            return NotFound($"Class '{className}' not found.");

        if (!Enum.TryParse<Ability>(request.Ability, ignoreCase: true, out var ability))
            return BadRequest(
                $"Invalid ability '{request.Ability}'. Valid values: {string.Join(", ", Enum.GetNames<Ability>())}");

        if (await _classRepository.SavingThrowExistsAsync(@class.Id, ability, ct))
            return Conflict($"Saving throw for '{ability}' already exists on class '{className}'.");

        var savingThrow = new ClassSavingThrowProficiency { ClassId = @class.Id, Ability = ability };
        await _classRepository.AddSavingThrowAsync(savingThrow, ct);
        await _classRepository.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetSavingThrows), new { className },
            CharacterMapper.ToClassSavingThrowDto(savingThrow));
    }

    // DELETE /api/classes/{className}/saving-throws/{id}
    [HttpDelete("{className}/saving-throws/{id:int}")]
    public async Task<IActionResult> DeleteSavingThrow(string className, int id, CancellationToken ct)
    {
        var savingThrow = await _classRepository.GetSavingThrowByIdAsync(id, ct);
        if (savingThrow is null || !savingThrow.Class.Name.Equals(className, StringComparison.OrdinalIgnoreCase))
            return NotFound();

        await _classRepository.DeleteSavingThrowAsync(savingThrow, ct);
        await _classRepository.SaveChangesAsync(ct);

        return NoContent();
    }
}