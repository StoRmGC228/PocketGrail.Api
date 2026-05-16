namespace PocketGrail.Application.Helpers;

public static class CacheKeys
{
    public static string VerificationCode(string email) => $"verify:{email}";
}
