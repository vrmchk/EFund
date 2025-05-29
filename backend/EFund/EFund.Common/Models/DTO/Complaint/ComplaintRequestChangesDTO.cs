namespace EFund.Common.Models.DTO.Complaint;

public class ComplaintRequestChangesDTO
{
    public Guid ComplaintId { get; set; }
    public string Message { get; set; } = string.Empty;
}