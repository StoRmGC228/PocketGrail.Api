namespace PocketGrail.Domain.Entities;

public class Proficiency : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ProficiencyType { get; set; } = "weapon";

    public ICollection<Character> Characters { get; set; } = [];
    public ICollection<CharacterProficiency> CharacterProficiencies { get; set; } = [];
}
