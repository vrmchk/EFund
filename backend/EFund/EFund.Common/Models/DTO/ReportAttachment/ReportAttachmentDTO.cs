namespace EFund.Common.Models.DTO.ReportAttachment;

public class ReportAttachmentDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
}