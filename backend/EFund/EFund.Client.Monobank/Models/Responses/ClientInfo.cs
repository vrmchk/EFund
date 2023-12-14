namespace EFund.Client.Monobank.Models.Responses;

public class ClientInfo
{
    public string ClientId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string WebHookUrl { get; set; } = string.Empty;

    public string Permissions { get; set; } = string.Empty;

    public List<Account> Accounts { get; set; } = null!;

    public List<Jar> Jars { get; set; } = null!;
}