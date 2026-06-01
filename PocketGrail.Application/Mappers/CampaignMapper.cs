namespace PocketGrail.Application.Mappers;

using PocketGrail.Application.DTOs;
using PocketGrail.DataAccess.Entities;

public static class CampaignMapper
{
    public static CampaignDto ToDto(Campaign c, bool includeParticipants) => new()
    {
        Id = c.Id,
        Name = c.Name,
        ShortDescription = c.ShortDescription,
        ConnectionCode = c.ConnectionCode,
        ImageUrl = c.ImageUrl,
        IsActive = c.IsActive,
        DmOwnerId = c.DmOwnerId,
        DmOwnerUsername = c.DmOwner?.Username ?? string.Empty,
        ParticipantCount = c.Participants.Count,
        CreatedAt = c.CreatedAt,
        Participants = includeParticipants
            ? c.Participants.Select(ToParticipantDto).ToList()
            : []
    };

    public static CampaignParticipantDto ToParticipantDto(CampaignParticipant p) => new()
    {
        UserId = p.UserId,
        Username = p.User?.Username ?? string.Empty,
        Role = p.Role.ToString()
    };
}
