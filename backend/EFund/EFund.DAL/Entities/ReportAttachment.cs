using System.ComponentModel.DataAnnotations.Schema;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class ReportAttachment : BaseEntity<Guid>
{
    public string FilePath { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public Guid FundraisingReportId { get; set; }

    [ForeignKey(nameof(FundraisingReportId))]
    public FundraisingReport FundraisingReport { get; set; } = null!;
}