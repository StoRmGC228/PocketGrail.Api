

namespace PocketGrail.Infrastructure;

using Microsoft.EntityFrameworkCore;

internal sealed class PocketGrailDbContext : DbContext
{
    public PocketGrailDbContext(DbContextOptions<PocketGrailDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}