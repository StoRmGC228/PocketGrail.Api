namespace PocketGrail.Domain.Entities.ClassEntities;

using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Characters;

public class ClassStartingItemChoicePair : BaseEntity
{
    public int ClassStartingItemSetId { get; set; }
    public ClassStartingItemSet Set { get; set; } = null!;
    public List<Item> OptionA { get; set; } = [];
    public List<Item> OptionB { get; set; } = [];
}
