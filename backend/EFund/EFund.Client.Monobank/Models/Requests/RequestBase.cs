namespace EFund.Client.Monobank.Models.Requests;

public abstract class RequestBase
{
    protected RequestBase(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}
