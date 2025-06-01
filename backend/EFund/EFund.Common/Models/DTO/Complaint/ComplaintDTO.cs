using EFund.Common.Enums;
using EFund.Common.Models.DTO.Violation;

namespace EFund.Common.Models.DTO.Complaint;

public class ComplaintDTO
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public ComplaintStatus Status { get; set; }
    public string? Comment { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }

    public Guid FundraisingId { get; set; }
    public Guid RequestedBy { get; set; }
    public Guid RequestedFor { get; set; }
    public Guid? ReviewedBy { get; set; }
    public string RequestedByUserName { get; set; } = string.Empty;
    public string RequestedForUserName { get; set; } = string.Empty;
    public string ReviewedByUserName { get; set; } = string.Empty;
    public List<ViolationDTO> Violations { get; set; } = [];
}