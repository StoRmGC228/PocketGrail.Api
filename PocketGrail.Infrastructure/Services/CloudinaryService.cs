namespace PocketGrail.Infrastructure.Services;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using PocketGrail.Application.Interfaces;

internal sealed class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder = "pocket-grail/campaigns", CancellationToken ct = default)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder,
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error is not null)
            throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");

        return result.SecureUrl.ToString();
    }
}
