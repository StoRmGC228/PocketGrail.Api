namespace PocketGrail.Domain.SupportingTypes;

using PocketGrail.Domain.Enums;

public sealed class CampaignMember
{
    public int UserId { get; }
    public string Username { get; }
    public UserRole Role { get; }

    public CampaignMember(int userId, string username, UserRole role)
    {
        UserId = userId;
        Username = username;
        Role = role;
    }
}
