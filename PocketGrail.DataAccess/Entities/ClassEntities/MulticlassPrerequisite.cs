namespace PocketGrail.DataAccess.Entities.ClassEntities;

using PocketGrail.DataAccess.Entities.Enums;

public class MulticlassPrerequisite : BaseEntity
{
    public int ClassId { get; set; }
    public Ability RequiredAbility { get; set; }
    public int MinimumScore { get; set; }
    public Class Class { get; set; } = null!;
}
