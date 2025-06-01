using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Notification;

public class NotificationDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationReason Reason { get; set; }
    public bool IsRead { get; set; }
    public object? Args { get; set; }
}