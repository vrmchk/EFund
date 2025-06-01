using EFund.Common.Models.Utility.Notifications;
using EFund.Common.Models.Utility.Notifications.Abstractions;

namespace EFund.Common.Enums;

public enum NotificationReason
{
    Empty,
    ComplaintResponseForRequestedBy,
    [NotificationArgs(typeof(ComplaintAcceptedForRequestedForArgs))]
    ComplaintAcceptedForRequestedFor,
    [NotificationArgs(typeof(ComplaintRequestChangesForRequestedForArgs))]
    ComplaintRequestChangesForRequestedFor
}