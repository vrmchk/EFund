using System.ComponentModel.DataAnnotations.Schema;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class MonobankFundraising : BaseEntity<Guid>
{
    public string JarId { get; set; } = string.Empty;
    public string SendId { get; set; } = string.Empty;
    
    public Guid FundraisingId { get; set; }

    [ForeignKey(nameof(FundraisingId))]
    public Fundraising Fundraising { get; set; } = null!;
}