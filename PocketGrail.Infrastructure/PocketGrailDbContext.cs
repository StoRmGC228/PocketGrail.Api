namespace PocketGrail.Infrastructure;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Domain.Entities;

internal sealed class PocketGrailDbContext : DbContext
{
    public PocketGrailDbContext(DbContextOptions<PocketGrailDbContext> options) : base(options)
    {
    }

    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<CampaignParticipant> CampaignParticipants => Set<CampaignParticipant>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PocketGrailDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}