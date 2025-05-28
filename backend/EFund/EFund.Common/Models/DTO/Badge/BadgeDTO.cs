using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Badge;

public class BadgeDTO
{
    public BadgeType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}