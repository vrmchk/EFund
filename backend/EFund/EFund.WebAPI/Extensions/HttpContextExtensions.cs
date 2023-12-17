namespace EFund.WebAPI.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext httpContext)
    {
        return Guid.Parse(httpContext.User.Claims.Single(c => c.Type == "id").Value);
    }

    public static string GetApiUrl(this HttpContext httpContext)
    {
        return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
    }
}