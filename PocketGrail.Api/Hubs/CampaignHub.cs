namespace PocketGrail.Api.Hubs;

using Microsoft.AspNetCore.SignalR;

public sealed class CampaignHub : Hub
{
    public async Task JoinCampaignGroup(string code)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, code);
    }

    public async Task LeaveCampaignGroup(string code)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, code);
    }

    public async Task NotifyParticipantJoined(string code, object participant)
    {
        await Clients.Group(code).SendAsync("ParticipantJoined", participant);
    }

    public async Task NotifyParticipantLeft(string code, object participantId)
    {
        await Clients.Group(code).SendAsync("ParticipantLeft", participantId);
    }
}
