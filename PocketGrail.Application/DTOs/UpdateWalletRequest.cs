namespace PocketGrail.Application.DTOs;

public sealed class UpdateWalletRequest
{
    public int? CpCoins { get; init; }
    public int? SpCoins { get; init; }
    public int? EpCoins { get; init; }
    public int? GpCoins { get; init; }
    public int? PpCoins { get; init; }
}
