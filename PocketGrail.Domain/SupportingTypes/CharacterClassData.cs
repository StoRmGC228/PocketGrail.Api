namespace PocketGrail.Domain.SupportingTypes;

public sealed class CharacterClassData
{
    public int Id { get; }            // CharacterClass junction record ID
    public int ClassId { get; }       // catalog Class.Id
    public string ClassName { get; }
    public int ClassLevel { get; private set; }
    public int HitDiceValue { get; }  // sides on the hit die, e.g. 8 for d8
    public int? SubclassId { get; private set; }
    public string? SubclassName { get; private set; }
    public int SkillChoiceCount { get; }
    public IReadOnlyList<string> AvailableSkillChoices { get; }
    public IReadOnlyList<string> AvailableSavingThrows { get; }
    public IReadOnlyList<string> MulticlassProficiencies { get; }
    public IReadOnlyList<ClassFeatureTemplate> AllFeatureTemplates { get; }
    public IReadOnlyList<SpellSlotTemplate> AllSpellSlotTemplates { get; }

    public CharacterClassData(
        int id,
        int classId,
        string className,
        int classLevel,
        int hitDiceValue,
        int? subclassId,
        string? subclassName,
        int skillChoiceCount,
        IReadOnlyList<string> availableSkillChoices,
        IReadOnlyList<string> availableSavingThrows,
        IReadOnlyList<string> multiclassProficiencies,
        IReadOnlyList<ClassFeatureTemplate> allFeatureTemplates,
        IReadOnlyList<SpellSlotTemplate> allSpellSlotTemplates)
    {
        Id = id;
        ClassId = classId;
        ClassName = className;
        ClassLevel = classLevel;
        HitDiceValue = hitDiceValue;
        SubclassId = subclassId;
        SubclassName = subclassName;
        SkillChoiceCount = skillChoiceCount;
        AvailableSkillChoices = availableSkillChoices;
        AvailableSavingThrows = availableSavingThrows;
        MulticlassProficiencies = multiclassProficiencies;
        AllFeatureTemplates = allFeatureTemplates;
        AllSpellSlotTemplates = allSpellSlotTemplates;
    }

    internal void IncrementLevel() => ClassLevel++;

    internal void SetSubclass(int subclassId, string subclassName)
    {
        SubclassId = subclassId;
        SubclassName = subclassName;
    }
}
