using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class SaveAcceptedComplaintNotificationForRequestedForJobArgs : IJobArgs
{
    public required Guid UserId { get; set; }
    public required string FundraisingTitle { get; set; }
    public required List<string> Violations { get; set; }
}