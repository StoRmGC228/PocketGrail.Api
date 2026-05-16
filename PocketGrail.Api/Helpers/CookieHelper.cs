namespace PocketGrail.Api.Helpers;

public static class CookieHelper
{
    private const string CookieName = "MySecretCookies";

    public static void AppendAuthCookie(HttpResponse response, string token) =>
        response.Cookies.Append(CookieName, token, BuildOptions(DateTimeOffset.UtcNow.AddDays(180)));

    public static void DeleteAuthCookie(HttpResponse response) =>
        response.Cookies.Delete(CookieName, BuildOptions());

    public static CookieOptions BuildOptions(DateTimeOffset? expires = null) =>
        new()
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.None,
            Path     = "/",
            Expires  = expires
        };
}
