namespace PocketGrail.Application.DTOs;

public sealed class FeatureDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string FeatureType { get; init; } = "class";
    public int? FeatureLevel { get; init; }
    public string? SourceClass { get; init; }
    public string? SourceRace { get; init; }
    public bool IsAutoAdded { get; init; }
}
