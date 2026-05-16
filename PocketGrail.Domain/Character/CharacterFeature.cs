namespace PocketGrail.Domain.Entities;

public class CharacterFeature
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public int FeatureId { get; set; }
    public Feature Feature { get; set; } = null!;

    public bool IsAutoAdded { get; set; }
}