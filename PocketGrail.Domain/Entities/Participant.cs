namespace PocketGrail.Domain.Entities;

using Enums;

public class Participant : BaseEntity
{
    public UserRole Role { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
