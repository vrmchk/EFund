using EFund.Common.Enums;
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
        var notification = new Notification
        {
            UserId = data.UserId,
            Reason = NotificationReason.ComplaintRequestChangesForRequestedFor,
            Message = data.Message
        };

        await _notificationRepository.InsertAsync(notification);
    }

    private string Message(SaveRequestedChangesComplaintNotificationForRequestedForJobArgs data) =>
        $"""
         We have temporarily hidden your fundraising campaign {data.FundraisingTitle} after a review by our moderation team.
         To restore visibility, please address the following: {data.Message}
         """;
}