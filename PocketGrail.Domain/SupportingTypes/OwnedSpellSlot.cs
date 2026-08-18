namespace PocketGrail.Domain.SupportingTypes;

public sealed class OwnedSpellSlot
{
    public int SlotLevel { get; }
    public int TotalSlots { get; private set; }
    public int RemainingSlots { get; private set; }

    public OwnedSpellSlot(int slotLevel, int totalSlots, int remainingSlots)
    {
        SlotLevel = slotLevel;
        TotalSlots = totalSlots;
        RemainingSlots = remainingSlots;
    }

    internal void UpdateTotalSlots(int newTotal)
    {
        var diff = newTotal - TotalSlots;
        TotalSlots = newTotal;
        RemainingSlots = Math.Min(RemainingSlots + diff, TotalSlots);
    }

    internal void SetRemaining(int remaining) => RemainingSlots = remaining;
}
