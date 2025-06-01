using EFund.Common.Enums;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;

namespace EFund.Hangfire.Jobs;

public class SaveComplaintResponseNotificationForRequestedByJob(
    IRepository<Notification> notificationRepository
)
    : IJob<SaveComplaintRepsonseNotificationForRequestedByJobArgs>
{
    private readonly IRepository<Notification> _notificationRepository = notificationRepository;

    public static string Id => nameof(SaveComplaintResponseNotificationForRequestedByJob);

    public async Task Run(SaveComplaintRepsonseNotificationForRequestedByJobArgs data, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = data.UserId,
            Reason = NotificationReason.ComplaintResponseForRequestedBy
        };

        await _notificationRepository.InsertAsync(notification);
    }
}