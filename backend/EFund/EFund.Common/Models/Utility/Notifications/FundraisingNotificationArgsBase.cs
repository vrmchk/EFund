using EFund.Common.Models.Utility.Notifications.Abstractions;

namespace EFund.Common.Models.Utility.Notifications;

public class FundraisingNotificationArgsBase : NotificationArgsBase
{
    public required string FundraisingTitle { get; set; }
    public required Guid FundraisingId { get; set; }
}