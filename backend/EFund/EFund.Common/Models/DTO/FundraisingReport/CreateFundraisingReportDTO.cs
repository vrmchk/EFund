namespace EFund.Common.Models.DTO.FundraisingReport;

public class CreateFundraisingReportDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid FundraisingId { get; set; }
}