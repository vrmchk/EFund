using EFund.Common.Models.Utility.Notifications.Abstractions;

namespace EFund.Common.Models.Utility.Notifications;

public class ComplaintAcceptedForRequestedForArgs : NotificationArgsBase
{
    public required string FundraisingTitle { get; set; }
    public required List<string> Violations { get; set; }
}