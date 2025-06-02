namespace EFund.Common.Models.Utility.Notifications;

public class FundraisingReviewedArgs : FundraisingNotificationArgsBase
{
    public required string Comment { get; set; }
    public required decimal RatingChange { get; set; }
}