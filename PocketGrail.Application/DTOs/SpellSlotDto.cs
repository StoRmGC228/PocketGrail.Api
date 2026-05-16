namespace PocketGrail.Application.DTOs;

public sealed class SpellSlotDto
{
    public int Id { get; init; }
    public int SlotLevel { get; init; }
    public int TotalSlots { get; init; }
    public int RemainingSlots { get; init; }
}
