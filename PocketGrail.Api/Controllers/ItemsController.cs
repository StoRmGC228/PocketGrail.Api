namespace PocketGrail.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;
using PocketGrail.Application.Mappers;
using PocketGrail.Domain.Entities.Characters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ItemsController : ControllerBase
{
    private readonly IItemRepository _itemRepository;

    public ItemsController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    // GET /api/items
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _itemRepository.GetAllAsync(ct);
        return Ok(items.Select(CharacterMapper.ToCatalogItemDto));
    }

    // GET /api/items/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var item = await _itemRepository.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(CharacterMapper.ToCatalogItemDto(item));
    }

    // POST /api/items
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCatalogItemRequest request, CancellationToken ct)
    {
        var item = new Item
        {
            Name            = request.Name,
            Description     = request.Description,
            Rarity          = request.Rarity ?? "Common",
            Category        = request.Category ?? "Gear",
            Weight          = request.Weight ?? 0f,
            Cost            = request.Cost,
            IsWeapon        = request.IsWeapon,
            IsMagical       = request.IsMagical,
            AtkMod          = request.AtkMod,
            Damage          = request.Damage,
            DamageType      = request.DamageType,
            WeaponProperties = request.WeaponProperties,
            ChargesInfo     = request.ChargesInfo,
            RechargeType    = request.RechargeType,
            Tags            = request.Tags,
            CreatedAt       = DateTime.UtcNow,
            UpdatedAt       = DateTime.UtcNow,
        };
        await _itemRepository.AddAsync(item, ct);
        await _itemRepository.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, CharacterMapper.ToCatalogItemDto(item));
    }

    // PUT /api/items/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCatalogItemRequest request, CancellationToken ct)
    {
        var item = await _itemRepository.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        if (request.Name is not null)             item.Name             = request.Name;
        if (request.Description is not null)      item.Description      = request.Description;
        if (request.Rarity is not null)           item.Rarity           = request.Rarity;
        if (request.Category is not null)         item.Category         = request.Category;
        if (request.Weight.HasValue)              item.Weight           = request.Weight.Value;
        if (request.Cost is not null)             item.Cost             = request.Cost;
        if (request.IsWeapon.HasValue)            item.IsWeapon         = request.IsWeapon.Value;
        if (request.IsMagical.HasValue)           item.IsMagical        = request.IsMagical.Value;
        if (request.AtkMod is not null)           item.AtkMod           = request.AtkMod;
        if (request.Damage is not null)           item.Damage           = request.Damage;
        if (request.DamageType is not null)       item.DamageType       = request.DamageType;
        if (request.WeaponProperties is not null) item.WeaponProperties = request.WeaponProperties;
        if (request.ChargesInfo is not null)      item.ChargesInfo      = request.ChargesInfo;
        if (request.RechargeType is not null)     item.RechargeType     = request.RechargeType;
        if (request.Tags is not null)             item.Tags             = request.Tags;
        item.UpdatedAt = DateTime.UtcNow;

        await _itemRepository.SaveChangesAsync(ct);
        return Ok(CharacterMapper.ToCatalogItemDto(item));
    }

    // DELETE /api/items/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await _itemRepository.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        await _itemRepository.DeleteAsync(item, ct);
        await _itemRepository.SaveChangesAsync(ct);
        return NoContent();
    }
}
