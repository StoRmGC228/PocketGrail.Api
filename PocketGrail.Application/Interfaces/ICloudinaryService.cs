namespace PocketGrail.Application.Interfaces;

using Microsoft.AspNetCore.Http;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file, CancellationToken ct = default);
}
