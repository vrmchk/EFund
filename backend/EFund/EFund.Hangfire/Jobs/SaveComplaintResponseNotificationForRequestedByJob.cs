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
            Reason = NotificationReason.ComplaintResponseForRequestedBy,
            Message = "We have reviewed the content you reported and confirmed that it violated our Terms of Use. As a result, the offending content has been successfully removed from the platform"
        };

        await _notificationRepository.InsertAsync(notification);
    }
}