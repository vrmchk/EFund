using EFund.Common.Models.DTO.ReportAttachment;

namespace EFund.Common.Models.DTO.FundraisingReport;

public class FundraisingReportDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid FundraisingId { get; set; }
    public List<ReportAttachmentDTO> Attachments { get; set; } = new();
}