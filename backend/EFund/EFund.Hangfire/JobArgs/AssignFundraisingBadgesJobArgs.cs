using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class AssignFundraisingBadgesJobArgs : IJobArgs
{
    public required Guid UserId { get; set; }
}