namespace PocketGrail.Application.Interfaces;

public interface IDatabaseSeeder
{
    Task SeedAsync(CancellationToken ct = default);
}
