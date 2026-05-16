namespace PocketGrail.Domain.Entities;

public class CharacterItem
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;

    public bool IsEquipped { get; set; }
    public bool IsAttuned { get; set; }
    public int Quantity { get; set; } = 1;
}