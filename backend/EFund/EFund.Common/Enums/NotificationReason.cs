using EFund.Common.Models.Utility.Notifications;
using EFund.Common.Models.Utility.Notifications.Abstractions;

namespace EFund.Common.Enums;

public enum NotificationReason
{
    Empty,
    [NotificationArgs(typeof(ComplaintResponseForRequestedByArgs))]
    ComplaintResponseForRequestedBy,
    [NotificationArgs(typeof(ComplaintAcceptedForRequestedForArgs))]
    ComplaintAcceptedForRequestedFor,
    [NotificationArgs(typeof(ComplaintRequestChangesForRequestedForArgs))]
    ComplaintRequestChangesForRequestedFor,
    [NotificationArgs(typeof(FundraisingStatusChangedArgs))]
    FundraisingHidden,
    [NotificationArgs(typeof(FundraisingStatusChangedArgs))]
    FundraisingDeleted,
    [NotificationArgs(typeof(FundraisingReviewedArgs))]
    FundraisingReviewed,
}