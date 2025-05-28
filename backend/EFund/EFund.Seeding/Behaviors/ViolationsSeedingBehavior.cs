using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Models;
using EFund.Seeding.Utils;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors;

[DependsOn([typeof(ViolationGroupsSeedingBehavior)])]
public class ViolationsSeedingBehavior(
    AppDataConfig appDataConfig,
    IRepository<Violation> violationRepository,
    IRepository<ViolationGroup> groupRepository
)
    : BaseSeedingBehavior(appDataConfig)
{
    private readonly IRepository<Violation> _violationRepository = violationRepository;
    private readonly IRepository<ViolationGroup> _groupRepository = groupRepository;

    public override async Task SeedAsync()
    {
        var groupsByTitle = (await _groupRepository.ToListAsync())
            .ToDictionary(g => g.Title, g => g.Id);

        var existingViolations = await _violationRepository.ToListAsync();
        var existingViolationTitles = existingViolations.Select(v => v.Title).ToList();

        var seedingViolations = SeedingUtils.GetFromCsv<ViolationSeedingDTO>($"{DataFolder}/violations.csv");
        var seedingViolationTitles = seedingViolations.Select(v => v.Title).ToList();

        var titlesToAdd = seedingViolationTitles.Except(existingViolationTitles);
        var titlesToRemove = existingViolationTitles.Except(seedingViolationTitles);

        await _violationRepository.Where(v => titlesToRemove.Contains(v.Title)).ExecuteDeleteAsync();

        var violationsToAdd = seedingViolations
            .Where(v => titlesToAdd.Contains(v.Title))
            .GroupBy(v => v.Group)
            .SelectMany(g => g.Select(v => new Violation
            {
                Title = v.Title,
                ViolationGroupId = groupsByTitle[v.Group],
            }));

        await _violationRepository.InsertManyAsync(violationsToAdd);
    }
}