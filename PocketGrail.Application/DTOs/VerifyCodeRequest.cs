namespace PocketGrail.Application.DTOs;

public sealed class VerifyCodeRequest
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
