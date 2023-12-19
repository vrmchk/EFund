using EFund.Common.Constants;

namespace EFund.WebAPI.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext httpContext)
    {
        return Guid.Parse(httpContext.User.Claims.Single(c => c.Type == Claims.Id).Value);
    }

    public static bool IsAdmin(this HttpContext httpContext)
    {
        return httpContext.User.Claims.Any(c => c is { Type: "role", Value: Roles.Admin });
    }

    public static string GetApiUrl(this HttpContext httpContext)
    {
        return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
    }
}