using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class SaveRequestedChangesComplaintNotificationForRequestedForJobArgs : IJobArgs
{
    public required Guid UserId { get; set; }
    public required string Message { get; set; }
    public required string FundraisingTitle { get; set; }
}