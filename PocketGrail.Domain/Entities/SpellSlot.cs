namespace PocketGrail.Domain.Entities;

public class SpellSlot : BaseEntity
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public int SlotLevel { get; set; }
    public int TotalSlots { get; set; }
    public int RemainingSlots { get; set; }
}