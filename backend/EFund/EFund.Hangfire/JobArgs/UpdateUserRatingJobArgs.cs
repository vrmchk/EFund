using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class UpdateUserRatingJobArgs : IJobArgs
{
    public required Guid UserId { get; set; }
    public required decimal RatingChange { get; set; }
}