using EFund.Common.Models.DTO.FundraisingReport;
using EFund.Common.Models.DTO.Monobank;

namespace EFund.Common.Models.DTO.Fundraising;

public class FundraisingDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string MonobankJarId { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public List<FundraisingReportDTO> Reports { get; set; } = new();
    public JarDTO? MonobankJar { get; set; }
}