namespace PocketGrail.Domain.SupportingTypes;

public sealed class OwnedItem
{
    public int ItemId { get; }
    public bool IsEquipped { get; private set; }
    public bool IsAttuned { get; private set; }
    public int Quantity { get; private set; }

    public OwnedItem(int itemId, bool isEquipped = false, bool isAttuned = false, int quantity = 1)
    {
        ItemId = itemId;
        IsEquipped = isEquipped;
        IsAttuned = isAttuned;
        Quantity = quantity;
    }

    internal void ToggleEquipped() => IsEquipped = !IsEquipped;
    internal void ToggleAttuned() => IsAttuned = !IsAttuned;
    internal void SetQuantity(int qty) => Quantity = qty;
}
