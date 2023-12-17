using Microsoft.AspNetCore.Identity;

namespace EFund.DAL.Entities;

public class User : IdentityUser<Guid>
{
    public User()
    {
        UserRegistrations = new List<UserRegistration>();
        UserMonobanks = new List<UserMonobank>();
    }
    
    public string DisplayName { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }

    public List<UserRegistration> UserRegistrations { get; set; }
    public List<UserMonobank> UserMonobanks { get; set; }
}