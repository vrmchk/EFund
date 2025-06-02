using System.ComponentModel.DataAnnotations.Schema;
using EFund.Common.Enums;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class Notification : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public NotificationReason Reason { get; set; }
    public bool IsRead { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? Args { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}