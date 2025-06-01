using EFund.Common.Models.Utility.Notifications.Abstractions;

namespace EFund.Common.Models.Utility.Notifications;

public class ComplaintRequestChangesForRequestedForArgs : NotificationArgsBase
{
    public required string Message { get; set; }
    public required string FundraisingTitle { get; set; }
}