namespace PocketGrail.Domain.Entities.Characters;

public class Item : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Rarity { get; set; } = "common";
    public string Category { get; set; } = "gear";

    public float Weight { get; set; }
    public string? Cost { get; set; }

    public bool IsWeapon { get; set; }
    public bool IsMagical { get; set; }
    public string? AtkMod { get; set; }
    public string? Damage { get; set; }
    public string? DamageType { get; set; }
    public string? WeaponProperties { get; set; }
    public string? ChargesInfo { get; set; }
    public string? RechargeType { get; set; }
    public string? Tags { get; set; }

    public ICollection<Character> Characters { get; set; } = [];
    public ICollection<CharacterItem> CharacterItems { get; set; } = [];
}
