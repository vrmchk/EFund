using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Complaint;

public class ComplaintTotalsDTO
{
    public Dictionary<ComplaintStatus, int> TotalsByStatus { get; set; } = new();
    public int OverallTotal { get; set; }
}