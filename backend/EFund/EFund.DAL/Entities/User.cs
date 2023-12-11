using Microsoft.AspNetCore.Identity;

namespace EFund.DAL.Entities;

public class User : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
}