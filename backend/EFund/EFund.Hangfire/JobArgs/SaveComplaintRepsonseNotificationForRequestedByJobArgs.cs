using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class SaveComplaintRepsonseNotificationForRequestedByJobArgs : IJobArgs
{
    public required Guid UserId { get; set; }
}