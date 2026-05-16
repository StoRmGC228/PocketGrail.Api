namespace PocketGrail.Application.DTOs;

using Microsoft.AspNetCore.Http;

public sealed class UpdateCharacterImageRequest
{
    public IFormFile Image { get; init; } = null!;
    public float CropX { get; init; }
    public float CropY { get; init; }
    public float CropWidth { get; init; }
    public float CropHeight { get; init; }
}
