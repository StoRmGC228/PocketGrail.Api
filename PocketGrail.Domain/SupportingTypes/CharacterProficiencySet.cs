namespace PocketGrail.Domain.SupportingTypes;

using PocketGrail.Domain.Enums;

public sealed class CharacterProficiencySet
{
    private readonly List<(Skill Skill, bool HasExpertise)> _skills = new();
    private readonly List<string> _weapons = new();
    private readonly List<string> _armors = new();
    private readonly List<string> _languages = new();
    private readonly List<string> _instruments = new();
    private readonly List<Ability> _savingThrows = new();

    public IReadOnlyList<(Skill Skill, bool HasExpertise)> Skills     => _skills.AsReadOnly();
    public IReadOnlyList<string>  Weapons                             => _weapons.AsReadOnly();
    public IReadOnlyList<string>  Armors                              => _armors.AsReadOnly();
    public IReadOnlyList<string>  Languages                           => _languages.AsReadOnly();
    public IReadOnlyList<string>  Instruments                        => _instruments.AsReadOnly();
    public IReadOnlyList<Ability> SavingThrows                       => _savingThrows.AsReadOnly();

    internal void AddSkill(Skill skill, bool hasExpertise)
    {
        if (!_skills.Any(s => s.Skill == skill))
            _skills.Add((skill, hasExpertise));
    }

    internal void AddWeapon(string name)
    {
        if (!_weapons.Any(w => w.Equals(name, StringComparison.OrdinalIgnoreCase)))
            _weapons.Add(name);
    }

    internal void AddArmor(string name)
    {
        if (!_armors.Any(a => a.Equals(name, StringComparison.OrdinalIgnoreCase)))
            _armors.Add(name);
    }

    internal void AddLanguage(string name)
    {
        if (!_languages.Any(l => l.Equals(name, StringComparison.OrdinalIgnoreCase)))
            _languages.Add(name);
    }

    internal void AddInstrument(string name)
    {
        if (!_instruments.Any(i => i.Equals(name, StringComparison.OrdinalIgnoreCase)))
            _instruments.Add(name);
    }

    internal void AddSavingThrow(Ability ability)
    {
        if (!_savingThrows.Contains(ability))
            _savingThrows.Add(ability);
    }

    public static CharacterProficiencySet Reconstitute(
        IEnumerable<(Skill Skill, bool HasExpertise)> skills,
        IEnumerable<string> weapons,
        IEnumerable<string> armors,
        IEnumerable<string> languages,
        IEnumerable<string> instruments,
        IEnumerable<Ability> savingThrows)
    {
        var set = new CharacterProficiencySet();
        foreach (var (skill, expertise) in skills)  set.AddSkill(skill, expertise);
        foreach (var w in weapons)                  set.AddWeapon(w);
        foreach (var a in armors)                   set.AddArmor(a);
        foreach (var l in languages)                set.AddLanguage(l);
        foreach (var i in instruments)              set.AddInstrument(i);
        foreach (var st in savingThrows)            set.AddSavingThrow(st);
        return set;
    }

    internal void RemoveSkill(Skill skill)       => _skills.RemoveAll(s => s.Skill == skill);
    internal void RemoveWeapon(string name)      => _weapons.RemoveAll(w => w.Equals(name, StringComparison.OrdinalIgnoreCase));
    internal void RemoveArmor(string name)       => _armors.RemoveAll(a => a.Equals(name, StringComparison.OrdinalIgnoreCase));
    internal void RemoveLanguage(string name)    => _languages.RemoveAll(l => l.Equals(name, StringComparison.OrdinalIgnoreCase));
    internal void RemoveInstrument(string name)  => _instruments.RemoveAll(i => i.Equals(name, StringComparison.OrdinalIgnoreCase));
    internal void RemoveSavingThrow(Ability ability) => _savingThrows.Remove(ability);
}
