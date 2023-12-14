namespace EFund.Common.Models.DTO.Monobank;

public class JarDTO
{
    public string Id { get; set; } = string.Empty;

    public string SendId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public decimal? Goal { get; set; }

}