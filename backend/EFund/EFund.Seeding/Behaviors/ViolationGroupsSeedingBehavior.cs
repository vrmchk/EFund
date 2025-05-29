using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Models;
using EFund.Seeding.Utils;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors;

public class ViolationGroupsSeedingBehavior(
    AppDataConfig appDataConfig,
    IRepository<ViolationGroup> groupRepository
)
    : BaseSeedingBehavior(appDataConfig)
{
    private readonly IRepository<ViolationGroup> _groupRepository = groupRepository;

    public override async Task SeedAsync()
    {
        var existingGroups = await _groupRepository.ToListAsync();
        var existingGroupTitle = existingGroups.Select(g => g.Title).ToList();

        var seedingViolations = SeedingUtils.GetFromCsv<ViolationSeedingDTO>($"{DataFolder}Violations.csv");
        var seedingGroupTitles = seedingViolations.Select(g => g.Group).ToList();

        var titlesToAdd = seedingGroupTitles.Except(existingGroupTitle);
        var titlesToRemove = existingGroupTitle.Except(seedingGroupTitles);

        await _groupRepository
            .Where(g => titlesToRemove.Contains(g.Title))
            .ExecuteUpdateAsync(s => s.SetProperty(g => g.IsDeleted, true));

        await _groupRepository.InsertManyAsync(titlesToAdd.Select(t => new ViolationGroup { Title = t, }));
    }
}