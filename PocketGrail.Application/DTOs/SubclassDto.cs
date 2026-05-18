namespace PocketGrail.Application.DTOs;

public sealed class SubclassDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? ShortDescription { get; init; }
    public int ClassId { get; init; }
}
