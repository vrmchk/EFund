namespace EFund.Client.Monobank.Models.Requests;

public class StatementRequest : RequestBase
{
    public StatementRequest(string token, string account, long from, long to) : base(token)
    {
        Account = account;
        From = from;
        To = to;
    }

    public StatementRequest(string token, string account, long from) : base(token)
    {
        Account = account;
        From = from;
    }

    public string Account { get; set; }

    public long From { get; set; }

    public long To { get; set; }
}
