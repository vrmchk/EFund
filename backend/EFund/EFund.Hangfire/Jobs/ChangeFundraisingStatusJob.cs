using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;
using Microsoft.EntityFrameworkCore;

namespace EFund.Hangfire.Jobs;

public class ChangeFundraisingStatusJob(
    IRepository<Fundraising> fundraisingRepository
)
    : IJob<ChangeFundraisingStatusJobArgs>
{
    private readonly IRepository<Fundraising> _fundraisingRepository = fundraisingRepository;

    public static string Id => nameof(ChangeFundraisingStatusJobArgs);

    public async Task Run(ChangeFundraisingStatusJobArgs data, CancellationToken cancellationToken = default)
    {
        var fundraising = await _fundraisingRepository.FirstOrDefaultAsync(f => f.Id == data.FundraisingId, cancellationToken);
        if (fundraising == null)
            return;

        fundraising.Status = data.FundraisingStatus;

        await _fundraisingRepository.UpdateAsync(fundraising);
    }
}