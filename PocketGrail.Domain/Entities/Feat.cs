using PocketGrail.Domain.Entities.Characters;

namespace PocketGrail.Domain.Entities;

public class Feat : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Requirement { get; set; }
    public string? Description { get; set; }

    public ICollection<Character> Characters { get; set; } = [];
}