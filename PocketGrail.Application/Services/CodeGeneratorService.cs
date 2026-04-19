namespace PocketGrail.Application.Services;

using System.Security.Cryptography;

public static class CodeGeneratorService
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int CodeLength = 6;

    public static string Generate()
    {
        return RandomNumberGenerator.GetString(Alphabet, CodeLength);
    }
}
