namespace PocketGrail.DataAccess.Entities.ClassEntities;

public class ClassSpellSlotTemplate : BaseEntity
{
    public int ClassId { get; set; }
    public int ClassLevel { get; set; }
    public int SpellSlotLevel { get; set; }
    public int TotalSlots { get; set; }
    public Class Class { get; set; } = null!;
}
