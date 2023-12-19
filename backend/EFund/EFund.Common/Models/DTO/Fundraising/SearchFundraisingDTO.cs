namespace EFund.Common.Models.DTO.Fundraising;

public class SearchFundraisingDTO
{
    public string? Title { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IncludeClosed { get; set; }
}