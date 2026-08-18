namespace PocketGrail.Application.DTOs;

public sealed class UpdateSpellSlotRequest
{
    public int SlotLevel { get; init; }
    public int RemainingSlots { get; init; }
}
