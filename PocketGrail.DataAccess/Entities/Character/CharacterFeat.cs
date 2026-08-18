namespace PocketGrail.DataAccess.Entities.Characters;

public class CharacterFeat
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public int FeatId { get; set; }
    public Feat Feat { get; set; } = null!;
}