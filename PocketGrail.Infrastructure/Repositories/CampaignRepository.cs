namespace PocketGrail.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Application.Interfaces;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Enums;

internal sealed class CampaignRepository : ICampaignRepository
{
    private readonly PocketGrailDbContext _context;

    public CampaignRepository(PocketGrailDbContext context)
    {
        _context = context;
    }

    public Task<Campaign?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _context.Campaigns
            .Include(c => c.DmOwner)
            .Include(c => c.Participants).ThenInclude(p => p.User)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Campaign?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        _context.Campaigns
            .Include(c => c.DmOwner)
            .Include(c => c.Participants).ThenInclude(p => p.User)
            .FirstOrDefaultAsync(c => c.ConnectionCode == code, ct);

    public async Task<IReadOnlyList<Campaign>> GetActiveAsync(CancellationToken ct = default) =>
        await _context.Campaigns
            .Include(c => c.DmOwner)
            .Include(c => c.Participants)
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Campaign>> GetByDmOwnerIdAsync(int dmUserId, CancellationToken ct = default) =>
        await _context.Campaigns
            .Include(c => c.DmOwner)
            .Include(c => c.Participants)
            .Where(c => c.DmOwnerId == dmUserId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Campaign>> GetByParticipantUserIdAsync(int userId, CancellationToken ct = default) =>
        await _context.Campaigns
            .Include(c => c.DmOwner)
            .Include(c => c.Participants)
            .Where(c => c.Participants.Any(p => p.UserId == userId && p.Role == UserRole.Player))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public Task<bool> CodeExistsAsync(string code, CancellationToken ct = default) =>
        _context.Campaigns.AnyAsync(c => c.ConnectionCode == code, ct);

    public Task<bool> IsUserParticipantAsync(int campaignId, int userId, CancellationToken ct = default) =>
        _context.CampaignParticipants.AnyAsync(p => p.CampaignId == campaignId && p.UserId == userId, ct);

    public async Task AddAsync(Campaign campaign, CancellationToken ct = default) =>
        await _context.Campaigns.AddAsync(campaign, ct);

    public async Task AddParticipantAsync(CampaignParticipant participant, CancellationToken ct = default) =>
        await _context.CampaignParticipants.AddAsync(participant, ct);

    public Task DeleteAsync(Campaign campaign, CancellationToken ct = default)
    {
        _context.Campaigns.Remove(campaign);
        return Task.CompletedTask;
    }

    public async Task RemoveParticipantAsync(int campaignId, int userId, CancellationToken ct = default)
    {
        var participant = await _context.CampaignParticipants
            .FirstOrDefaultAsync(p => p.CampaignId == campaignId && p.UserId == userId, ct);
        if (participant is not null)
            _context.CampaignParticipants.Remove(participant);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
