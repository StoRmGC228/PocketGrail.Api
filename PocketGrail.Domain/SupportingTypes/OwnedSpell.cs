namespace PocketGrail.Domain.SupportingTypes;

public sealed class OwnedSpell
{
    public int SpellId { get; }
    public int SpellLevel { get; }
    public bool IsPrepared { get; private set; }

    public OwnedSpell(int spellId, int spellLevel, bool isPrepared = true)
    {
        SpellId = spellId;
        SpellLevel = spellLevel;
        IsPrepared = isPrepared;
    }

    internal void TogglePrepared() => IsPrepared = !IsPrepared;
}
