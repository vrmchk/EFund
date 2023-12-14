using System.ComponentModel.DataAnnotations.Schema;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;
public class UserMonobank : BaseEntity<Guid>
{
    public Guid UserId { get; set; }

    public byte[] MonobankToken { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
