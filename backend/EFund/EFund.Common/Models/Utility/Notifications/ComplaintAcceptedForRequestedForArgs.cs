namespace EFund.Common.Models.Utility.Notifications;

public class ComplaintAcceptedForRequestedForArgs : FundraisingNotificationArgsBase
{
    public required List<string> Violations { get; set; }
}