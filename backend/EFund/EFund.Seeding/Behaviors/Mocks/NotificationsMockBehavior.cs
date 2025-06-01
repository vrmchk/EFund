using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors.Mocks;

[DependsOn([typeof(UsersMockBehavior)])]
public class NotificationsMockBehavior(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig,
    UserManager<User> userManager)
    : BaseMockBehavior(appDataConfig, generalConfig)
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly string[] _userEmails = ["ihverwork@gmail.com", "vladd.golovatyuk@gmail.com", "tralala@gmail.com", "croco@gmail.com"];
    private readonly NotificationReason[] _notificationReasons = Enum.GetValues<NotificationReason>();
    private readonly Random _random = new(217);
    private readonly string[] _sampleMessages = [
        "Your fundraising report has been approved.",
        "New complaint submitted against your campaign.",
        "Your donation has been received. Thank you!",
        "Reminder: update your campaign with recent progress.",
        "New message from EFund support team.",
        "Violation review completed for your campaign.",
        "Fundraising goal reached!",
        "Suspicious activity noticed. Please verify your account."
    ];

    protected override async Task MockData()
    {
        var users = await _userManager.Users.Where(u => _userEmails.Contains(u.Email)).ToListAsync();
        foreach (var user in users)
        {
            if (user.Notifications.Count > 0)
                continue;

            var notifications = GetRandomNotifications(user);

            user.Notifications.AddRange(notifications);
            await _userManager.UpdateAsync(user);
        }
    }

    private List<Notification> GetRandomNotifications(User user)
    {
        var notifications = new List<Notification>();
        var notificationCount = _random.Next(1, 6);
        for (int i = 0; i < notificationCount; i++)
        {
            var notification = new Notification
            {
                UserId = user.Id,
                Reason = _notificationReasons[_random.Next(_notificationReasons.Length)],
                IsRead = false,
                Message = _sampleMessages[_random.Next(_sampleMessages.Length)],
            };

            notifications.Add(notification);
        }
        
        return notifications;
    }
}