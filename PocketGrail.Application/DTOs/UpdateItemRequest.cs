namespace PocketGrail.Application.DTOs;

public sealed class UpdateItemRequest
{
    public bool? IsEquipped { get; init; }
    public bool? IsAttuned { get; init; }
    public int? Quantity { get; init; }
}
