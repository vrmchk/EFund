using Microsoft.AspNetCore.Identity;

namespace EFund.DAL.Entities;

public class User : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = null!;
    public string? Description { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
    public string? AvatarPath { get; set; }
    public bool CreatedByAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal Rating { get; set; }

    public List<UserRegistration> UserRegistrations { get; set; } = [];
    public List<UserMonobank> UserMonobanks { get; set; } = [];
    public List<Badge> Badges { get; set; } = [];
    public List<Notification> Notifications { get; set; } = [];
}