using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors.Mocks;

[DependsOn([typeof(FundraisingsMockBehavior), typeof(ViolationsSeedingBehavior)])]
public class ComplaintsMockBehavior(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig,
    IRepository<Complaint> complaintRepository,
    IRepository<Violation> violationRepository,
    IRepository<Fundraising> fundraisingRepository) 
    : BaseMockBehavior(appDataConfig, generalConfig)
{
    private readonly IRepository<Complaint> _complaintRepository = complaintRepository;
    private readonly IRepository<Violation> _violationRepository = violationRepository;
    private readonly IRepository<Fundraising> _fundraisingRepository = fundraisingRepository;
    private readonly Random _random = new(217);
    private readonly string[] _requestedForUserEmails = ["croco@gmail.com", "tralala@gmail.com"];
    private readonly string[] _requestedForByEmails = ["ihverwork@gmail.com", "vladd.golovatyuk@gmail.com"];
    private readonly string[] _sampleComments = [
        "Suspected misuse of funds.",
        "Report seems inconsistent.",
        "Cannot verify provided receipts.",
        "No updates for a long time.",
        "Looks like a duplicate campaign."
    ];

    protected override async Task MockData()
    {
        var fundraisings = await _fundraisingRepository
            .Include(f => f.Complaints)
            .Include(f => f.User)
            .Where(f => _requestedForUserEmails.Contains(f.User.Email))
            .ToListAsync();

        var violations = await _violationRepository.Where(v => !v.IsDeleted).ToListAsync();
        var requestedForUsers = await _fundraisingRepository
            .Where(f => _requestedForUserEmails.Contains(f.User.Email))
            .Select(f => f.User)
            .ToListAsync();
        
        foreach (var fundraising in fundraisings)
        {
            if (fundraising.Complaints.Count > 0)
                continue;
            
            var complaints = GetRandomComplaints(fundraising, violations, requestedForUsers);
            await _complaintRepository.InsertManyAsync(complaints);
        }
    }
    
    private List<Complaint> GetRandomComplaints(
        Fundraising fundraising,
        List<Violation> violations,
        List<User> requestedForUsers)
    {
        var complaints = new List<Complaint>();
        int complaintCount = _random.Next(1, 4);

        for (int i = 0; i < complaintCount; i++)
        {
            bool includeComment = _random.NextDouble() < 0.8;
            bool includeViolations = _random.NextDouble() < 0.7;

            if (!includeComment && !includeViolations)
            {
                if (_random.NextDouble() < 0.5)
                    includeComment = true;
                else
                    includeViolations = true;
            }

            var selectedViolations = includeViolations
                ? violations
                    .OrderBy(_ => _random.Next())
                    .Take(_random.Next(1, 3))
                    .ToList()
                : [];

            var complaint = new Complaint
            {
                Status = ComplaintStatus.Pending,
                Comment = includeComment ? GenerateRandomComment() : null,
                RequestedAt = DateTimeOffset.UtcNow.AddDays(-_random.Next(10, 100)),
                FundraisingId = fundraising.Id,
                RequestedBy = requestedForUsers[_random.Next(requestedForUsers.Count)].Id,
                RequestedFor = fundraising.UserId,
                Violations = selectedViolations
            };

            complaints.Add(complaint);
        }

        return complaints;
    }

    private string GenerateRandomComment()
    {
        return _sampleComments[_random.Next(_sampleComments.Length)];
    }
}