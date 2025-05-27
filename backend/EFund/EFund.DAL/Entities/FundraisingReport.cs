using System.ComponentModel.DataAnnotations.Schema;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class FundraisingReport : BaseEntity<Guid>
{
    public FundraisingReport()
    {
        Attachments = new List<ReportAttachment>();
    }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

    public Guid FundraisingId { get; set; }

    [ForeignKey(nameof(FundraisingId))]
    public Fundraising Fundraising { get; set; }

    public List<ReportAttachment> Attachments { get; set; }
}