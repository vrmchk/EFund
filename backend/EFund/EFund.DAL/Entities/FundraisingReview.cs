using System.ComponentModel.DataAnnotations.Schema;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class FundraisingReview : BaseEntity<Guid>
{
    public decimal RatingChange { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

    public Guid ReviewedBy { get; set; }
    public Guid FundraisingId { get; set; }

    [ForeignKey(nameof(ReviewedBy))] 
    public User ReviwedByUser { get; set; } = null!;
    
    [ForeignKey(nameof(FundraisingId))]
    public Fundraising Fundraising { get; set; } = null!;
}