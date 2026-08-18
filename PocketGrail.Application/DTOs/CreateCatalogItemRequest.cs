namespace PocketGrail.Application.DTOs;

public sealed class CreateCatalogItemRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Rarity { get; init; }
    public string? Category { get; init; }
    public float? Weight { get; init; }
    public string? Cost { get; init; }
    public bool IsWeapon { get; init; }
    public bool IsMagical { get; init; }
    public string? AtkMod { get; init; }
    public string? Damage { get; init; }
    public string? DamageType { get; init; }
    public string? WeaponProperties { get; init; }
    public string? ChargesInfo { get; init; }
    public string? RechargeType { get; init; }
    public string? Tags { get; init; }
}
