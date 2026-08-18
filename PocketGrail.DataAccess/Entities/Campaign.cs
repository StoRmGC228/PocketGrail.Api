using PocketGrail.DataAccess.Entities.Characters;

namespace PocketGrail.DataAccess.Entities;

public class Campaign : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string ConnectionCode { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int DmOwnerId { get; set; }
    public User DmOwner { get; set; } = null!;
    public ICollection<CampaignParticipant> Participants { get; set; } = new List<CampaignParticipant>();
    public ICollection<Character> Characters { get; set; } = [];
}