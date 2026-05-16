namespace PocketGrail.Application.DTOs;

public sealed class ItemDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Rarity { get; init; } = "common";
    public string Category { get; init; } = "gear";
    public float Weight { get; init; }
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

    // Junction state
    public bool IsEquipped { get; init; }
    public bool IsAttuned { get; init; }
    public int Quantity { get; init; } = 1;
}
