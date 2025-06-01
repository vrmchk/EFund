using System.Text.Json;
using EFund.Common.Enums;
using EFund.Common.Models.Utility.Notifications;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;

namespace EFund.Hangfire.Jobs;

public class SaveAcceptedComplaintNotificationForRequestedForJob(
    IRepository<Notification> notificationRepository
)
    : IJob<SaveAcceptedComplaintNotificationForRequestedForJobArgs>
{
    private readonly IRepository<Notification> _notificationRepository = notificationRepository;

    public static string Id => nameof(SaveAcceptedComplaintNotificationForRequestedForJob);

    public async Task Run(SaveAcceptedComplaintNotificationForRequestedForJobArgs data, CancellationToken cancellationToken = default)
    {
        var args = new ComplaintAcceptedForRequestedForArgs { FundraisingTitle = data.FundraisingTitle, Violations = data.Violations };
        var notification = new Notification
        {
            UserId = data.UserId,
            Reason = NotificationReason.ComplaintAcceptedForRequestedFor,
            Args = JsonSerializer.Serialize(args),
        };

        await _notificationRepository.InsertAsync(notification);
    }
}