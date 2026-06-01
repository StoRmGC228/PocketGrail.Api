namespace PocketGrail.DataAccess.Entities;

using PocketGrail.DataAccess.Entities.Characters;

public class Feature : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }
}