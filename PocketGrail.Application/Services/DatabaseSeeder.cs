namespace PocketGrail.Application.Services;

using PocketGrail.Application.Interfaces;

public sealed class DatabaseSeeder : IDatabaseSeeder
{
    public Task SeedAsync(CancellationToken ct = default) => Task.CompletedTask;
}
