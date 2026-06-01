namespace PocketGrail.Domain.ValueObjects;

using PocketGrail.Domain.Exceptions;

public sealed record CharacterWallet(int Cp, int Sp, int Ep, int Gp, int Pp)
{
    public static CharacterWallet Create(int cp, int sp, int ep, int gp, int pp)
    {
        if (cp < 0 || sp < 0 || ep < 0 || gp < 0 || pp < 0)
            throw new DomainException("Coin count cannot be negative.");
        return new(cp, sp, ep, gp, pp);
    }

    public static CharacterWallet Empty => new(0, 0, 0, 0, 0);
}
