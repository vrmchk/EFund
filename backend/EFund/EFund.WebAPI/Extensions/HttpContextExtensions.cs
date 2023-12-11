namespace EFund.WebAPI.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext httpContext)
    {
        return Guid.Parse(httpContext.User.Claims.Single(c => c.Type == "id").Value);
    }
}