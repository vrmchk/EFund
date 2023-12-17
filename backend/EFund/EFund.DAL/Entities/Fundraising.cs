using System.ComponentModel.DataAnnotations.Schema;
using EFund.Common.Enums;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class Fundraising : BaseEntity<Guid>
{
    public Fundraising()
    {
        Tags = new List<Tag>();
    }

    public FundraisingProvider Provider { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public MonobankFundraising MonobankFundraising { get; set; } = null!;

    public List<Tag> Tags { get; set; } 
}