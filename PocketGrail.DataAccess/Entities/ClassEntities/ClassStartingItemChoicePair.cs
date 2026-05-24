namespace PocketGrail.DataAccess.Entities.ClassEntities;

using PocketGrail.DataAccess.Entities;
using PocketGrail.DataAccess.Entities.Characters;

public class ClassStartingItemChoicePair : BaseEntity
{
    public int ClassStartingItemSetId { get; set; }
    public ClassStartingItemSet Set { get; set; } = null!;
    public List<Item> OptionA { get; set; } = [];
    public List<Item> OptionB { get; set; } = [];
}
