namespace PocketGrail.Domain.ValueObjects;

using PocketGrail.Domain.Exceptions;

public sealed record CharacterStats(int Strength, int Dexterity, int Constitution, int Intelligence, int Wisdom, int Charisma)
{
    public static CharacterStats Create(int str, int dex, int con, int @int, int wis, int cha)
    {
        if (str < 0 || dex < 0 || con < 0 || @int < 0 || wis < 0 || cha < 0)
            throw new DomainException("Ability score cannot be negative.");
        return new(str, dex, con, @int, wis, cha);
    }

    public static CharacterStats Empty => new(10, 10, 10, 10, 10, 10);

    public int GetModifier(int score) => (score - 10) / 2;
    public int StrengthModifier     => GetModifier(Strength);
    public int DexterityModifier    => GetModifier(Dexterity);
    public int ConstitutionModifier => GetModifier(Constitution);
    public int IntelligenceModifier => GetModifier(Intelligence);
    public int WisdomModifier       => GetModifier(Wisdom);
    public int CharismaModifier     => GetModifier(Charisma);
}
