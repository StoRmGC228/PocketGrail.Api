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
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PocketGrailDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}