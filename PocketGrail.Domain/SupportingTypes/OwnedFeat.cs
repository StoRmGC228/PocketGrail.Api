namespace PocketGrail.Domain.SupportingTypes;

public sealed class OwnedFeat
{
    public int FeatId { get; }

    public OwnedFeat(int featId)
    {
        FeatId = featId;
    }
}
