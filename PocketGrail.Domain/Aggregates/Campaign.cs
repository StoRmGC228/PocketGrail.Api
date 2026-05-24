namespace PocketGrail.Domain.Aggregates;

using PocketGrail.Domain.Enums;
using PocketGrail.Domain.Exceptions;
using PocketGrail.Domain.SupportingTypes;

public sealed class Campaign
{
    // ── Properties ───────────────────────────────────────────────────────────────
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string ShortDescription { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string ConnectionCode { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public int DmOwnerId { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<CampaignMember> _participants;
    public IReadOnlyList<CampaignMember> Participants => _participants.AsReadOnly();

    // ── Constructor ──────────────────────────────────────────────────────────────
    private Campaign(
        int id,
        string name,
        string shortDescription,
        string passwordHash,
        string connectionCode,
        string? imageUrl,
        int dmOwnerId,
        bool isActive,
        List<CampaignMember> participants)
    {
        Id = id;
        Name = name;
        ShortDescription = shortDescription;
        PasswordHash = passwordHash;
        ConnectionCode = connectionCode;
        ImageUrl = imageUrl;
        DmOwnerId = dmOwnerId;
        IsActive = isActive;
        _participants = participants;
    }

    // ── Factory: reconstitute from persistence ────────────────────────────────
    public static Campaign Reconstitute(
        int id,
        string name,
        string shortDescription,
        string passwordHash,
        string connectionCode,
        string? imageUrl,
        int dmOwnerId,
        bool isActive,
        List<CampaignMember> participants) =>
        new(id, name, shortDescription, passwordHash, connectionCode,
            imageUrl, dmOwnerId, isActive, participants);

    // ── Factory: create new campaign ──────────────────────────────────────────
    public static Campaign Create(
        string name,
        string shortDescription,
        string passwordHash,
        string connectionCode,
        string? imageUrl,
        int dmOwnerId) =>
        new(0, name, shortDescription, passwordHash, connectionCode,
            imageUrl, dmOwnerId, isActive: true, participants: new());

    // ── Methods ──────────────────────────────────────────────────────────────────

    // Password verification is done by the Application layer before calling Join.
    // This method enforces only structural invariants.
    public void Join(int userId, string username)
    {
        if (userId == DmOwnerId)
            throw new DomainException("The campaign owner cannot join as a participant.");

        if (_participants.Any(p => p.UserId == userId))
            throw new DomainException("User is already a participant in this campaign.");

        _participants.Add(new CampaignMember(userId, username, UserRole.Player));
    }

    public void Leave(int userId)
    {
        if (userId == DmOwnerId)
            throw new DomainException("The campaign owner cannot leave their own campaign.");

        var member = _participants.FirstOrDefault(p => p.UserId == userId)
            ?? throw new DomainException("User is not a participant in this campaign.");

        _participants.Remove(member);
    }

    public void SetConnectionCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 6 || !code.All(char.IsLetterOrDigit))
            throw new DomainException("Connection code must be exactly 6 alphanumeric characters.");
        ConnectionCode = code.ToUpperInvariant();
    }

    public void UpdateImage(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new DomainException("Image URL cannot be empty.");
        ImageUrl = url;
    }

    public void Activate()   => IsActive = true;
    public void Deactivate() => IsActive = false;
}
