namespace PocketGrail.Application.Providers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PocketGrail.Application.Interfaces;
using PocketGrail.DataAccess.Entities;

public sealed class JwtProvider : IJwtProvider
{
    public Task<string> GenerateTokenAsync(User user)
    {
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET")
            ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set.");
        var issuer   = Environment.GetEnvironmentVariable("JWT_ISSUER")   ?? "PocketGrail";
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "PocketGrailClient";

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name,           user.Username),
            new(ClaimTypes.Role,           user.Role.ToString())
        };

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:             issuer,
            audience:           audience,
            claims:             claims,
            expires:            DateTime.UtcNow.AddDays(180),
            signingCredentials: creds);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
