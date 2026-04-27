namespace PocketGrail.Domain.Entities;

public class Session : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
}
