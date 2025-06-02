using AutoMapper;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.DTO.Notification;
using EFund.Common.Models.Utility.Notifications;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFund.BLL.Services;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(IRepository<Notification> notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<List<NotificationDTO>> GetListAsync(Guid userId, bool withRead, Guid? fundraisingId)
    {
        var notifications = await _notificationRepository
            .Where(n => n.UserId == userId && (withRead || !n.IsRead))
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var dtos = _mapper.Map<List<NotificationDTO>>(notifications);
        if (fundraisingId.HasValue)
        {
            dtos = dtos.Where(n => n.Args is FundraisingNotificationArgsBase args && args.FundraisingId == fundraisingId).ToList();
        }

        return dtos ;
    }

    public async Task SetIsRead(Guid id)
    {
        var notification = await _notificationRepository.FirstOrDefaultAsync(n => n.Id == id);
        if (notification is { IsRead: false })
        {
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
        }
    }

    public async Task BatchSetIsRead(BatchSetNotificationIsReadDTO dto)
    {
        await _notificationRepository
            .Where(n => !n.IsRead && dto.NotificationIds.Contains(n.Id))
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
    }
}