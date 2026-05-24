namespace PocketGrail.DataAccess.Entities.Characters;

using ClassEntities;

public class CharacterClass : BaseEntity
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public int ClassLevel { get; set; }
    public int TotalHitDiceCount { get; set; }
    public int? CharacterSubclassId { get; set; }
    public Subclass? CharacterSubclass { get; set; }
}
