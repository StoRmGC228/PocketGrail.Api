namespace PocketGrail.DataAccess.Entities;

using Enums;

public class CampaignParticipant : BaseEntity
{
    public UserRole Role { get; set; }
    public int CampaignId { get; set; }
    public Campaign Campaign { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
