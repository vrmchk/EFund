namespace EFund.Client.Monobank.Models.Responses;

public class Transaction
{
    public string Id { get; set; } = string.Empty;

    public long Time { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Mcc { get; set; }

    public int OriginalMcc { get; set; }

    public long Amount { get; set; }

    public long OperationAmount { get; set; }

    public int CurrencyCode { get; set; }

    public int CommissionRate { get; set; }

    public long CashbackAmount { get; set; }

    public long Balance { get; set; }

    public bool Hold { get; set; }

    public string ReceiptId { get; set; } = string.Empty;
}