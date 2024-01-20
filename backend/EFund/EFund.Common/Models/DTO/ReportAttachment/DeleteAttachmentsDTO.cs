namespace EFund.Common.Models.DTO.ReportAttachment;

public class DeleteAttachmentsDTO
{
    public List<Guid> AttachmentIds { get; set; } = new();
}