namespace PocketGrail.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketGrail.Application.Mappers;
using PocketGrail.DataAccess.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class RacesController : ControllerBase
{
    private readonly IRaceRepository _raceRepository;

    public RacesController(IRaceRepository raceRepository)
    {
        _raceRepository = raceRepository;
    }

    // GET /api/races
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var races = await _raceRepository.GetAllAsync(ct);
        return Ok(races.Select(CharacterMapper.ToRaceDto).ToList());
    }

    // GET /api/races/{name}
    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name, CancellationToken ct)
    {
        var race = await _raceRepository.GetByNameWithDetailsAsync(name, ct);
        return race is null ? NotFound() : Ok(CharacterMapper.ToRaceDto(race));
    }
}
