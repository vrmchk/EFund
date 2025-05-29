using EFund.Common.Enums;
using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.JobArgs;

public class ChangeFundraisingStatusJobArgs : IJobArgs
{
    public required Guid FundraisingId { get; set; }
    public required FundraisingStatus FundraisingStatus { get; set; }
}