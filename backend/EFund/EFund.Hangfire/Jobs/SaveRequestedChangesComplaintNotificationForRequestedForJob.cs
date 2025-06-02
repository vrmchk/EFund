using System.Text.Json;
using EFund.Common.Enums;
using EFund.Common.Models.Utility.Notifications;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;

namespace EFund.Hangfire.Jobs;

public class SaveRequestedChangesComplaintNotificationForRequestedForJob(
    IRepository<Notification> notificationRepository
)
    : IJob<SaveRequestedChangesComplaintNotificationForRequestedForJobArgs>
{
    private readonly IRepository<Notification> _notificationRepository = notificationRepository;

    public static string Id => nameof(SaveRequestedChangesComplaintNotificationForRequestedForJob);

    public async Task Run(SaveRequestedChangesComplaintNotificationForRequestedForJobArgs data,
        CancellationToken cancellationToken = default)
    {
        var args = new ComplaintRequestChangesForRequestedForArgs
        {
            FundraisingTitle = data.FundraisingTitle,
            FundraisingId = data.FundraisingId,
            Message = data.Message
        };
        var notification = new Notification
        {
            UserId = data.UserId,
            Reason = NotificationReason.ComplaintRequestChangesForRequestedFor,
            Args = JsonSerializer.Serialize(args)
        };

        await _notificationRepository.InsertAsync(notification);
    }
}