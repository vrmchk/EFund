namespace EFund.Common.Models.DTO.Complaint;

public class CreateComplaintDTO
{
    public Guid FundraisingId { get; set; }
    public string? Comment { get; set; }
    public List<Guid> ViolationIds { get; set; } = [];
}