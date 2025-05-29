using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Notification;

public class NotificationDTO
{
    public Guid UserId { get; set; }
    public NotificationReason Reason { get; set; }
    public bool IsRead { get; set; }
    public string Message { get; set; } = null!;
}

public class BatchSetNotificationIsReadDTO
{
    public List<Guid> NotificationIds { get; set; } = [];
}