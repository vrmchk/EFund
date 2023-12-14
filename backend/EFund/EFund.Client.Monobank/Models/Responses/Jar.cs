namespace EFund.Client.Monobank.Models.Responses;

public class Jar
{
    public string Id { get; set; } = string.Empty;

    public string SendId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int CurrencyCode { get; set; }

    public decimal Balance { get; set; }

    public decimal? Goal { get; set; }
}
