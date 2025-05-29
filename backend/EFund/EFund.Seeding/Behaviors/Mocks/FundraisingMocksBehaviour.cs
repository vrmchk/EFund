using System.Text.Json;
using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using EFund.Seeding.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors.Mocks;

[DependsOn([typeof(UsersMockBehavior), typeof(TagsMockBehaviour)])]
public class FundraisingsMocksBehaviour(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig,
    IRepository<Fundraising> fundraisingRepository,
    UserManager<User> userManager,
    IRepository<Tag> tagRepository)
    : BaseMockBehavior(appDataConfig, generalConfig)
{
    private readonly IRepository<Fundraising> _fundraisingRepository = fundraisingRepository;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IRepository<Tag> _tagRepository = tagRepository; 

    protected override async Task MockData()
    {
        var json = await File.ReadAllTextAsync($"{DataFolder}Fundraisings.json");
        var fundraisings = JsonSerializer.Deserialize<FundraisingMockDTO[]>(
            json) ?? throw new InvalidOperationException("Failed to deserialize Fundraisings data.");

        List<Fundraising> fundraisingsToAdd = [];
        var tagNames = fundraisings
            .SelectMany(f => f.Fundraising.Tags)
            .DistinctBy(t => t.Name)
            .Select(t => t.Name)
            .ToList();

        var tagsFromDb = await _tagRepository
            .Where(t => tagNames.Contains(t.Name))
            .ToListAsync();

        foreach (var fundraising in fundraisings)
        {
            var exists = await _fundraisingRepository.AnyAsync(f => f.Title == fundraising.Fundraising.Title);
            if (exists)
                continue;

            var user = await _userManager.FindByEmailAsync(fundraising.UserEmail)
                       ?? throw new InvalidOperationException($"User with email {fundraising.UserEmail} not found.");

            fundraising.Fundraising.UserId = user.Id;
            fundraising.Fundraising.Tags = tagsFromDb.Where(t => fundraising.Fundraising.Tags.Any(ft => ft.Name == t.Name)).ToList();

            if (fundraising.Fundraising.Status == FundraisingStatus.Closed)
            {
                fundraising.Fundraising.ClosedAt = fundraising.Fundraising.CreatedAt.AddDays(10);
            }
            else if (fundraising.Fundraising.Status == FundraisingStatus.ReadyForReview)
            {
                fundraising.Fundraising.ClosedAt = fundraising.Fundraising.CreatedAt.AddDays(10);
                fundraising.Fundraising.ReadyForReviewAt = fundraising.Fundraising.CreatedAt.AddDays(30);
            }

            fundraisingsToAdd.Add(fundraising.Fundraising);
        }

        await _fundraisingRepository.InsertManyAsync(fundraisingsToAdd);
    }
}