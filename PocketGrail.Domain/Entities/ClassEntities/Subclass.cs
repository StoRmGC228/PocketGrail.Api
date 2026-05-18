namespace PocketGrail.Domain.Entities.ClassEntities;

public class Subclass : BaseEntity
{
    public int ClassId { get; set; }
    public Class SourceClass { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
}
