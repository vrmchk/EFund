using EFund.Common.Enums;

namespace EFund.Common.Models.Utility.Notifications;

public class FundraisingStatusChangedArgs : FundraisingNotificationArgsBase
{
    public required string Comment { get; set; }
    public required FundraisingStatus From { get; set; }
    public required FundraisingStatus To { get; set; }
}