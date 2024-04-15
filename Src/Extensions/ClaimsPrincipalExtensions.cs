using System.Security.Claims;

namespace courses_dotnet_api.Src.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserRut(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    public static string GetUserRole(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    }
}
