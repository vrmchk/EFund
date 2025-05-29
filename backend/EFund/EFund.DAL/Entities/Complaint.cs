using System.ComponentModel.DataAnnotations.Schema;
using EFund.Common.Enums;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class Complaint : BaseEntity<Guid>
{
    public ComplaintStatus Status { get; set; }
    public string? Comment { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }

    public Guid FundraisingId { get; set; }
    public Guid RequestedBy { get; set; }
    public Guid RequestedFor { get; set; }
    public Guid? ReviewedBy { get; set; }

    [ForeignKey(nameof(FundraisingId))]
    public Fundraising Fundraising { get; set; } = null!;

    [ForeignKey(nameof(RequestedBy))]
    public User RequestedByUser { get; set; } = null!;

    [ForeignKey(nameof(RequestedFor))]
    public User RequestedForUser { get; set; } = null!;

    [ForeignKey(nameof(ReviewedBy))]
    public User? ReviewedByUser { get; set; }

    public List<Violation> Violations { get; set; } = [];
}