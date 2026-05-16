namespace PocketGrail.Domain.Entities;

public class Feature : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string FeatureType { get; set; } = "class";
    public int? FeatureLevel { get; set; }
    public string? SourceClass { get; set; }
    public string? SourceRace { get; set; }

    public ICollection<Character> Characters { get; set; } = [];
    public ICollection<CharacterFeature> CharacterFeatures { get; set; } = [];
}
