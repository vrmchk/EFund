using EFund.Common.Enums;
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
        var notification = new Notification
        {
            UserId = data.UserId,
            Reason = NotificationReason.ComplaintAcceptedForRequestedFor,
            Message = Message(data)
        };

        await _notificationRepository.InsertAsync(notification);
    }

    private string Message(SaveAcceptedComplaintNotificationForRequestedForJobArgs data) =>
        $"""
         We are writing to inform you that content associated with your fundraising {data.FundraisingTitle} has been removed following a review by our moderation team.
         The content was found to be in violation of our Terms of Use, specifically under the categories: {FormatViolations(data.Violations)}
         """;

    private string FormatViolations(List<string> violations) => string.Join(", ", violations.Select(v => $"\"{v}\""));
}