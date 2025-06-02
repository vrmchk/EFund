using EFund.Common.Enums;
using EFund.Common.Models.Utility.Notifications.Abstractions;
using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class SaveNotificationJobArgs : IJobArgs
{
    public Guid UserId { get; set; }
    public required NotificationReason Reason { get; set; }
    public required NotificationArgsBase? Args { get; set; }
}