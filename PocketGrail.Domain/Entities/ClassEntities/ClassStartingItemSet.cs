namespace PocketGrail.Domain.Entities.ClassEntities;

using PocketGrail.Domain.Entities;

public class ClassStartingItemSet : BaseEntity
{
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public List<ClassStartingItemChoicePair> ChoicePairs { get; set; } = [];
}
