using EFund.Common.Enums;
using EFund.Common.Models.DTO.Violation;

namespace EFund.Common.Models.DTO.Complaint;

public class ComplaintDTO
{
    public Guid Id { get; set; }
    public ComplaintStatus Status { get; set; }
    public string? Comment { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }

    public Guid FundraisingId { get; set; }
    public Guid RequestedBy { get; set; }
    public Guid RequestedFor { get; set; }
    public Guid? ReviewedBy { get; set; }
    public List<ViolationDTO> Violations { get; set; } = [];
}