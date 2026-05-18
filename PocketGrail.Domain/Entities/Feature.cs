namespace PocketGrail.Domain.Entities;

using PocketGrail.Domain.Entities.Characters;

public class Feature : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }
}