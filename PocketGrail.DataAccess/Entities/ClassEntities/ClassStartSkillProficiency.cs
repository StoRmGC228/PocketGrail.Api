namespace PocketGrail.DataAccess.Entities.ClassEntities;

using PocketGrail.DataAccess.Entities;
using PocketGrail.DataAccess.Entities.Enums;

public class ClassStartSkillProficiency : BaseEntity
{
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public Skill Skill { get; set; }
}
