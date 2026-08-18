namespace PocketGrail.Application.DTOs;

public sealed class FeatDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Requirement { get; init; }
    public string? Description { get; init; }
}
