using EFund.Common.Enums;
using EFund.Common.Models.DTO.FundraisingReport;
using EFund.Common.Models.DTO.Monobank;

namespace EFund.Common.Models.DTO.Fundraising;

public class FundraisingDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public FundraisingStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }
    public DateTimeOffset? ReadyForReviewAt { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatarUrl { get; set; } = string.Empty;
    public string MonobankJarId { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public List<FundraisingReportDTO> Reports { get; set; } = new();
    public JarDTO? MonobankJar { get; set; }
}