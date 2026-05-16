namespace PocketGrail.Api.Helpers;

using System.Security.Claims;

public static class ClaimsHelper
{
    public static int GetUserId(ClaimsPrincipal user)
    {
        var raw = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User id claim missing.");
        return int.Parse(raw);
    }
}
