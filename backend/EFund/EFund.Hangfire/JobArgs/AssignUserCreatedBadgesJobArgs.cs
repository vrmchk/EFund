using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class AssignUserCreatedBadgesJobArgs : IJobArgs
{
    public Guid UserId { get; set; }
}