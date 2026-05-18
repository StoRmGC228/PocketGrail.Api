namespace PocketGrail.Domain.Entities.ClassEntities;

using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Enums;

public class ClassStartSkillProficiency : BaseEntity
{
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public Skill Skill { get; set; }
}
