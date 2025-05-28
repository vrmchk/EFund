using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class AssignFundraisingBadgesJobArgs : IJobArgs
{
    public Guid UserId { get; set; }
}