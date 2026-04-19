namespace PocketGrail.Infrastructure;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Domain.Entities;

internal sealed class PocketGrailDbContext : DbContext
{
    public PocketGrailDbContext(DbContextOptions<PocketGrailDbContext> options) : base(options)
    {
    }

    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Participant> Participants => Set<Participant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PocketGrailDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}