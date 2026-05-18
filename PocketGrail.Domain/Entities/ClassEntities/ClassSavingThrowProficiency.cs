namespace PocketGrail.Domain.Entities.ClassEntities;

using PocketGrail.Domain.Entities.Enums;

public class ClassSavingThrowProficiency : BaseEntity
{
    public int ClassId { get; set; }
    public Ability Ability { get; set; }
    public Class Class { get; set; } = null!;
}
