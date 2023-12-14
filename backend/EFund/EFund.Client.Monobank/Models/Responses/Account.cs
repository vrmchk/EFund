namespace EFund.Client.Monobank.Models.Responses;

public class Account
{
    public string Id { get; set; } = string.Empty;

    public string SendId { get; set; } = string.Empty;

    public int CurrencyCode { get; set; }

    public string CashbackType { get; set; } = string.Empty;

    public long Balance { get; set; }

    public long CreditLimit { get; set; }

    public List<string> MaskedPan { get; set; } = null!;

    public string Type { get; set; } = string.Empty;

    public string Iban { get; set; } = string.Empty;
}
