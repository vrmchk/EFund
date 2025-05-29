using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class AssignUserCreatedBadgesJobArgs : IJobArgs
{
    public required Guid UserId { get; set; }
}