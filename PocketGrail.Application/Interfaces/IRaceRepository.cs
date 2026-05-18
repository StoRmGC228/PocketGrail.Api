namespace PocketGrail.Application.Interfaces;

using PocketGrail.Domain.Entities;

public interface IRaceRepository
{
    Task<IReadOnlyList<Race>> GetAllAsync(CancellationToken ct = default);
    Task<Race?> GetByNameWithDetailsAsync(string name, CancellationToken ct = default);
}
