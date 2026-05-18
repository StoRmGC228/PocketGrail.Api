namespace PocketGrail.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Mappers;

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
}
