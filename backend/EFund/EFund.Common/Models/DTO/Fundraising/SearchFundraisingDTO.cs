using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Fundraising;

public class SearchFundraisingDTO
{
    public string? Title { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<FundraisingStatus> Statuses { get; set; } = [];
    public Guid? UserId { get; set; }
}