using System.Text.Json;
using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using EFund.Seeding.Models;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors.Mocks;

public class TagsMockBehaviour(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig,
    IRepository<Tag> tagRepository)
    : BaseMockBehavior(appDataConfig, generalConfig)
{
    private readonly IRepository<Tag> _tagRepository = tagRepository;
    
    protected override async Task MockData()
    {
        var json = await File.ReadAllTextAsync($"{DataFolder}Fundraisings.json");
        var fundraisings = JsonSerializer.Deserialize<FundraisingMockDTO[]>(
            json) ?? throw new InvalidOperationException("Failed to deserialize Fundraisings data.");

        var tags = fundraisings
            .SelectMany(f => f.Fundraising.Tags)
            .DistinctBy(t => t.Name)
            .Select(t => new Tag { Name = t.Name.ToLower() })
            .ToList();

        var tagNames = tags.Select(t => t.Name).ToList();

        var tagsFromDb = await _tagRepository.Where(t => tagNames.Contains(t.Name)).Select(t => t.Name).ToListAsync();
        var tagsToAdd = tags
            .Where(t => !tagsFromDb.Contains(t.Name))
            .ToList();
        
        if (tagsToAdd.Count > 0)
            await _tagRepository.InsertManyAsync(tagsToAdd);
    }
}