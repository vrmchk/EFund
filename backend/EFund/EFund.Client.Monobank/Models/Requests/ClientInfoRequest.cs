namespace EFund.Client.Monobank.Models.Requests;

public class ClientInfoRequest : RequestBase
{
    public ClientInfoRequest(string token) : base(token)
    {
    }
}