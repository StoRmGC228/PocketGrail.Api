namespace PocketGrail.DataAccess.Entities.ClassEntities;

using PocketGrail.DataAccess.Entities.Enums;

public class ClassSavingThrowProficiency : BaseEntity
{
    public int ClassId { get; set; }
    public Ability Ability { get; set; }
    public Class Class { get; set; } = null!;
}
