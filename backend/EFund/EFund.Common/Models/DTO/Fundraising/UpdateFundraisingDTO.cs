namespace EFund.Common.Models.DTO.Fundraising;

public class UpdateFundraisingDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MonobankJarId { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
}