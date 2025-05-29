using EFund.Common.Models.DTO.Badge;
using EFund.Common.Models.DTO.Notification;

namespace EFund.Common.Models.DTO.User;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
    public bool HasPassword { get; set; }
    public bool HasMonobankToken { get; set; }
    public bool IsAdmin { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal Rating { get; set; }
    public List<BadgeDTO> Badges { get; set; } = [];
    public List<NotificationDTO> Notifications { get; set; } = [];
}