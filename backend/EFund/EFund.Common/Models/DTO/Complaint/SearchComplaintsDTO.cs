using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Complaint;

public class SearchComplaintsDTO
{
    public ComplaintStatus? Status { get; set; }
    public Guid RequestedBy { get; set; }
    public Guid? ReviewedBy { get; set; }
}