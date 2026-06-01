namespace PocketGrail.DataAccess.Entities;

public class RaceFeature : Feature
{
    public int RaceId { get; set; }
    public Race SourceRace { get; set; } = null!;
}
