namespace PocketGrail.Domain.Entities.Characters;

public class CharacterWallet : BaseEntity
{
    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public int CpCoins { get; set; }
    public int SpCoins { get; set; }
    public int EpCoins { get; set; }
    public int GpCoins { get; set; }
    public int PpCoins { get; set; }
}