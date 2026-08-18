namespace PocketGrail.DataAccess.Entities.Characters;

public class CharacterSpell
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public int SpellId { get; set; }
    public Spell Spell { get; set; } = null!;

    public bool Prepared { get; set; } = true;
}