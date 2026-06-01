namespace PocketGrail.DataAccess.Entities.ClassEntities;

using PocketGrail.DataAccess.Entities.Proficiencies;

public class SubclassFeature : Feature
{
    public int GainingLevel { get; set; }

    public int SubclassId { get; set; }
    public Subclass SourceSubclass { get; set; } = null!;

    public List<WeaponProficiency>  WeaponGrants     { get; set; } = [];
    public List<ArmorProficiency>   ArmorGrants      { get; set; } = [];
    public List<Language>           LanguageGrants   { get; set; } = [];
    public List<Instrument>         InstrumentGrants { get; set; } = [];
}
