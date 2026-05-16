namespace PocketGrail.Domain.Entities;

public class CharacterClass : BaseEntity
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public string ClassName { get; set; } = string.Empty;
    public int ClassLevel { get; set; } = 1;
    public string HitDice { get; set; } = string.Empty;
    public string? Subclass { get; set; }
    public int TotalHitDice { get; set; }
    public int UsedHitDice { get; set; }
}