using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors;

public class BadgesSeedingBehaviour(
    AppDataConfig appDataConfig,
    IRepository<Badge> badgeRepository)
    : BaseSeedingBehavior(appDataConfig)
{
    private readonly IRepository<Badge> _badgeRepository = badgeRepository;

    public override async Task SeedAsync()
    {
        var badgesTypesFromDb = (await _badgeRepository.ToListAsync()).Select(b => b.Type).ToList();

        var badgeTypes = Enum.GetValues<BadgeType>().Where(bt => bt != BadgeType.None).ToList();

        var badgesToAdd = badgeTypes.Except(badgesTypesFromDb).Select(bt => new Badge
        {
            Type = bt
        });

        await _badgeRepository.InsertManyAsync(badgesToAdd);
    }
}