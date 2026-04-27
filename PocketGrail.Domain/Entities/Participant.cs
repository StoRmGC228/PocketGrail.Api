namespace PocketGrail.Domain.Entities;

using Enums;

public class Participant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public DateTime JoinedAt { get; set; }
}
