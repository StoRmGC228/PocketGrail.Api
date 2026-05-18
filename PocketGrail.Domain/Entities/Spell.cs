using PocketGrail.Domain.Entities.Characters;

namespace PocketGrail.Domain.Entities;

public class Spell : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public string? School { get; set; }
    public string? Range { get; set; }
    public string? CastingTime { get; set; }
    public bool Concentration { get; set; }
    public bool IsRitual { get; set; }
    public string? Components { get; set; }

    public ICollection<Character> Characters { get; set; } = [];
    public ICollection<CharacterSpell> CharacterSpells { get; set; } = [];
}