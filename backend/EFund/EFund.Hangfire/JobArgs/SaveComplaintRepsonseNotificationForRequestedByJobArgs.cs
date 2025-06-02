using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class SaveComplaintRepsonseNotificationForRequestedByJobArgs : IJobArgs
{
    public required Guid UserId { get; set; }
    public required string FundraisingTitle { get; set; }
    public required Guid FundraisingId { get; set; }
}