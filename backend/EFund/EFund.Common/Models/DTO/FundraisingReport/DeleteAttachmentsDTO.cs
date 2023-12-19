namespace EFund.Common.Models.DTO.FundraisingReport;

public class DeleteAttachmentsDTO
{
    public List<Guid> AttachmentIds { get; set; } = new();
}