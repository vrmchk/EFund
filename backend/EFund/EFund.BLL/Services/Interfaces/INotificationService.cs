using EFund.Common.Models.DTO.Notification;

namespace EFund.BLL.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDTO>> GetListAsync(Guid userId, bool withRead, Guid? fundraisingId);
    Task SetIsRead(Guid id);
    Task BatchSetIsRead(BatchSetNotificationIsReadDTO dto);
}