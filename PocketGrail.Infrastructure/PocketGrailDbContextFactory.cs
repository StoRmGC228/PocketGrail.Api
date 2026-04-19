namespace PocketGrail.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

internal sealed class PocketGrailDbContextFactory : IDesignTimeDbContextFactory<PocketGrailDbContext>
{
    public PocketGrailDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("POCKET_GRAIL_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=pocketgrail_dev;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<PocketGrailDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new PocketGrailDbContext(options);
    }
}
