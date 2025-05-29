using System.ComponentModel.DataAnnotations.Schema;
using EFund.Common.Enums;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class Notification : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public NotificationReason Reason { get; set; }
    public bool IsRead { get; set; }
    public string Message { get; set; } = null!;
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}