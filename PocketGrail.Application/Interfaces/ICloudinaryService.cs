namespace PocketGrail.Application.Interfaces;

using Microsoft.AspNetCore.Http;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file, string folder = "pocket-grail/campaigns", CancellationToken ct = default);
}
