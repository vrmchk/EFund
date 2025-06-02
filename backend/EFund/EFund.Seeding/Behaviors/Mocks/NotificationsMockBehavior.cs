using System.Text.Json;
using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.Common.Models.Utility.Notifications;
using EFund.Common.Models.Utility.Notifications.Abstractions;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors.Mocks;

[DependsOn([typeof(FundraisingsMockBehavior), typeof(ViolationsSeedingBehavior)])]
public class NotificationsMockBehavior(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig,
    UserManager<User> userManager,
    IRepository<Fundraising> fundraisingRepository,
    IRepository<Violation> violationRepository)
    : BaseMockBehavior(appDataConfig, generalConfig)
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IRepository<Fundraising> _fundraisingRepository = fundraisingRepository;
    private readonly IRepository<Violation> _violationRepository = violationRepository;

    private readonly string[] _userEmails = ["ihverwork@gmail.com", "vladd.golovatyuk@gmail.com", "tralala@gmail.com", "croco@gmail.com"];

    private readonly NotificationReason[] _notificationReasons = Enum.GetValues<NotificationReason>();
    private readonly Random _random = new(217);

    protected override async Task MockData()
    {
        var users = await _userManager.Users.Where(u => _userEmails.Contains(u.Email)).ToListAsync();
        var fundraisings = await _fundraisingRepository.Include(f => f.User).ToListAsync();
        var violations = await _violationRepository.ToListAsync();
        foreach (var user in users)
        {
            if (user.Notifications.Count > 0)
                continue;

            var userFundraisings = fundraisings.Where(f => f.UserId == user.Id).ToList();
            var otherFundraisings = fundraisings.Where(f => f.UserId != user.Id).ToList();
            var notifications = GetRandomNotifications(user, userFundraisings, otherFundraisings, violations);

            user.Notifications.AddRange(notifications);
            await _userManager.UpdateAsync(user);
        }
    }

    private List<Notification> GetRandomNotifications(User user, List<Fundraising> userFundraisings, List<Fundraising> otherFundraisings, List<Violation> violations)
    {
        var notifications = new List<Notification>();
        var notificationCount = _random.Next(1, 6);
        for (int i = 0; i < notificationCount; i++)
        {
            var reason = PickRandom(_notificationReasons);
            var args = GetNotificationArgs(reason, PickRandom(userFundraisings), PickRandom(otherFundraisings), PickRandom(violations, 2));
            var notification = new Notification
            {
                UserId = user.Id,
                Reason = reason,
                IsRead = false,
                Args = args is not null ? JsonSerializer.Serialize(args) : null
            };

            notifications.Add(notification);
        }

        return notifications;
    }

    private NotificationArgsBase? GetNotificationArgs(NotificationReason reason, Fundraising userFundraising, Fundraising otherFundraising, List<Violation> violations)
    {
        return reason switch
        {
            NotificationReason.ComplaintResponseForRequestedBy => new ComplaintResponseForRequestedByArgs
            {
                FundraisingTitle = otherFundraising.Title,
                FundraisingId = otherFundraising.Id
            },
            NotificationReason.ComplaintRequestChangesForRequestedFor => new ComplaintRequestChangesForRequestedForArgs
            {
                Message = "Remove inappropriate image from the report of the fundraising.",
                FundraisingTitle = userFundraising.Title,
                FundraisingId = userFundraising.Id
            },
            NotificationReason.ComplaintAcceptedForRequestedFor => new ComplaintAcceptedForRequestedForArgs
            {
                FundraisingTitle = userFundraising.Title,
                Violations = violations.Select(v => v.Title).ToList(),
                FundraisingId = userFundraising.Id
            },
            _ => null
        };
    }
}