namespace PocketGrail.Application.DTOs;

public sealed class FeatureDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
