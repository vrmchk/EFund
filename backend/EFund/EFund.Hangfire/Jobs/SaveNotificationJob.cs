using System.Text.Json;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;

namespace EFund.Hangfire.Jobs;

public class SaveNotificationJob(
    IRepository<Notification> notificationRepository
)
    : IJob<SaveNotificationJobArgs>
{
    private readonly IRepository<Notification> _notificationRepository = notificationRepository;

    public static string Id => nameof(SaveNotificationJob);

    public Task Run(SaveNotificationJobArgs data, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = data.UserId,
            Reason = data.Reason,
            CreatedAt = DateTimeOffset.Now,
            Args = data.Args != null ? JsonSerializer.Serialize(data.Args) : null
        };

        return _notificationRepository.InsertAsync(notification);
    }
}