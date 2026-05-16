namespace PocketGrail.Application.DTOs;

public sealed class AddFeatureRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string FeatureType { get; init; } = "class";
    public int? FeatureLevel { get; init; }
}
