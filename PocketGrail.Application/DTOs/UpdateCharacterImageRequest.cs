namespace PocketGrail.Application.DTOs;

using Microsoft.AspNetCore.Http;

public sealed class UpdateCharacterImageRequest
{
    public IFormFile Image { get; init; } = null!;
}
